using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common;
using TaskFlow.Application.Projects;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Enums;

namespace TaskFlow.Infrastructure.Persistence;

internal class ProjectService(AppDbContext db) : IProjectService
{
    private static readonly Expression<Func<ProjectMember, ProjectDto>> ToDto = m => new ProjectDto(
        m.Project.Id, m.Project.Name, m.Project.Description, m.Project.Deadline,
        m.Project.OwnerId, m.Role, m.Project.Members.Count, m.Project.CreatedAt);

    private Task<ProjectMember?> GetMembershipAsync(Guid userId, Guid projectId) =>
        db.ProjectMembers.AsNoTracking()
            .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);

    private static ServiceResult<T> NotFound<T>() =>
        ServiceResult<T>.Fail(ServiceStatus.NotFound, "Project not found.");

    private static ServiceResult<T> Forbidden<T>(string message) =>
        ServiceResult<T>.Fail(ServiceStatus.Forbidden, message);

    public async Task<ServiceResult<ProjectDto>> CreateAsync(Guid userId, CreateProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return ServiceResult<ProjectDto>.Fail(ServiceStatus.Invalid, "Name is required.");

        var project = new Project
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Deadline = request.Deadline?.ToUniversalTime(),
            OwnerId = userId,
            Members = [new ProjectMember { UserId = userId, Role = ProjectRole.Owner }]
        };

        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return await GetAsync(userId, project.Id);
    }

    public async Task<ServiceResult<List<ProjectDto>>> ListAsync(Guid userId)
    {
        var list = await db.ProjectMembers.AsNoTracking()
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.Project.CreatedAt)
            .Select(ToDto)
            .ToListAsync();
        return ServiceResult<List<ProjectDto>>.Ok(list);
    }

    public async Task<ServiceResult<ProjectDto>> GetAsync(Guid userId, Guid projectId)
    {
        var dto = await db.ProjectMembers.AsNoTracking()
            .Where(m => m.UserId == userId && m.ProjectId == projectId)
            .Select(ToDto)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound<ProjectDto>() : ServiceResult<ProjectDto>.Ok(dto);
    }

    public async Task<ServiceResult<ProjectDto>> UpdateAsync(Guid userId, Guid projectId, UpdateProjectRequest request)
    {
        var membership = await GetMembershipAsync(userId, projectId);
        if (membership is null) return NotFound<ProjectDto>();
        if (membership.Role < ProjectRole.Admin) return Forbidden<ProjectDto>("Admin role required.");
        if (string.IsNullOrWhiteSpace(request.Name))
            return ServiceResult<ProjectDto>.Fail(ServiceStatus.Invalid, "Name is required.");

        var project = await db.Projects.FirstAsync(p => p.Id == projectId);
        project.Name = request.Name.Trim();
        project.Description = request.Description?.Trim();
        project.Deadline = request.Deadline?.ToUniversalTime();
        await db.SaveChangesAsync();

        return await GetAsync(userId, projectId);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid userId, Guid projectId)
    {
        var membership = await GetMembershipAsync(userId, projectId);
        if (membership is null) return NotFound<bool>();
        if (membership.Role != ProjectRole.Owner) return Forbidden<bool>("Only the owner can delete a project.");

        await db.Projects.Where(p => p.Id == projectId).ExecuteDeleteAsync();
        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<List<MemberDto>>> ListMembersAsync(Guid userId, Guid projectId)
    {
        if (await GetMembershipAsync(userId, projectId) is null) return NotFound<List<MemberDto>>();

        var members = await (
            from m in db.ProjectMembers.AsNoTracking()
            where m.ProjectId == projectId
            join u in db.Users on m.UserId equals u.Id
            orderby m.Role descending, u.DisplayName
            select new MemberDto(u.Id, u.Email ?? "", u.DisplayName, m.Role, m.JoinedAt)
        ).ToListAsync();

        return ServiceResult<List<MemberDto>>.Ok(members);
    }

    public async Task<ServiceResult<MemberDto>> AddMemberAsync(Guid userId, Guid projectId, AddMemberRequest request)
    {
        var caller = await GetMembershipAsync(userId, projectId);
        if (caller is null) return NotFound<MemberDto>();
        if (caller.Role < ProjectRole.Admin) return Forbidden<MemberDto>("Admin role required.");
        if (request.Role == ProjectRole.Owner)
            return ServiceResult<MemberDto>.Fail(ServiceStatus.Invalid, "Cannot assign the Owner role.");
        if (request.Role == ProjectRole.Admin && caller.Role != ProjectRole.Owner)
            return Forbidden<MemberDto>("Only the owner can grant Admin.");

        var normalized = request.Email.Trim().ToUpperInvariant();
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.NormalizedEmail == normalized);
        if (user is null)
            return ServiceResult<MemberDto>.Fail(ServiceStatus.NotFound, "No user with that email.");

        if (await db.ProjectMembers.AnyAsync(m => m.ProjectId == projectId && m.UserId == user.Id))
            return ServiceResult<MemberDto>.Fail(ServiceStatus.Invalid, "User is already a member.");

        var member = new ProjectMember { ProjectId = projectId, UserId = user.Id, Role = request.Role };
        db.ProjectMembers.Add(member);
        await db.SaveChangesAsync();

        return ServiceResult<MemberDto>.Ok(
            new MemberDto(user.Id, user.Email ?? "", user.DisplayName, member.Role, member.JoinedAt));
    }

    public async Task<ServiceResult<bool>> RemoveMemberAsync(Guid userId, Guid projectId, Guid memberUserId)
    {
        var caller = await GetMembershipAsync(userId, projectId);
        if (caller is null) return NotFound<bool>();

        var target = await GetMembershipAsync(memberUserId, projectId);
        if (target is null)
            return ServiceResult<bool>.Fail(ServiceStatus.NotFound, "Member not found.");
        if (target.Role == ProjectRole.Owner)
            return ServiceResult<bool>.Fail(ServiceStatus.Invalid, "The owner cannot be removed.");

        if (memberUserId != userId)
        {
            if (caller.Role < ProjectRole.Admin) return Forbidden<bool>("Admin role required.");
            if (target.Role == ProjectRole.Admin && caller.Role != ProjectRole.Owner)
                return Forbidden<bool>("Only the owner can remove an Admin.");
        }

        await db.ProjectMembers
            .Where(m => m.ProjectId == projectId && m.UserId == memberUserId)
            .ExecuteDeleteAsync();
        return ServiceResult<bool>.Ok(true);
    }
}
