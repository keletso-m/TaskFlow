using TaskFlow.Application.Common;
using TaskFlow.Core.Enums;

namespace TaskFlow.Application.Projects;

public record CreateProjectRequest(string Name, string? Description, DateTime? Deadline);
public record UpdateProjectRequest(string Name, string? Description, DateTime? Deadline);
public record AddMemberRequest(string Email, ProjectRole Role);

public record ProjectDto(
    Guid Id, string Name, string? Description, DateTime? Deadline,
    Guid OwnerId, ProjectRole MyRole, int MemberCount, DateTime CreatedAt);

public record MemberDto(Guid UserId, string Email, string DisplayName, ProjectRole Role, DateTime JoinedAt);

public interface IProjectService
{
    Task<ServiceResult<ProjectDto>> CreateAsync(Guid userId, CreateProjectRequest request);
    Task<ServiceResult<List<ProjectDto>>> ListAsync(Guid userId);
    Task<ServiceResult<ProjectDto>> GetAsync(Guid userId, Guid projectId);
    Task<ServiceResult<ProjectDto>> UpdateAsync(Guid userId, Guid projectId, UpdateProjectRequest request);
    Task<ServiceResult<bool>> DeleteAsync(Guid userId, Guid projectId);
    Task<ServiceResult<List<MemberDto>>> ListMembersAsync(Guid userId, Guid projectId);
    Task<ServiceResult<MemberDto>> AddMemberAsync(Guid userId, Guid projectId, AddMemberRequest request);
    Task<ServiceResult<bool>> RemoveMemberAsync(Guid userId, Guid projectId, Guid memberUserId);
}
