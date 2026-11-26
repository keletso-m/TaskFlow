using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Application.Projects;

namespace TaskFlow.API.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public class ProjectsController(IProjectService projects) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirst("sub")!.Value);

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request)
    {
        var r = await projects.CreateAsync(UserId, request);
        return Map(r, v => CreatedAtAction(nameof(Get), new { id = v.Id }, v));
    }

    [HttpGet]
    public async Task<IActionResult> List() => Map(await projects.ListAsync(UserId));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id) => Map(await projects.GetAsync(UserId, id));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProjectRequest request) =>
        Map(await projects.UpdateAsync(UserId, id, request));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Map(await projects.DeleteAsync(UserId, id), _ => NoContent());

    [HttpGet("{id:guid}/members")]
    public async Task<IActionResult> Members(Guid id) => Map(await projects.ListMembersAsync(UserId, id));

    [HttpPost("{id:guid}/members")]
    public async Task<IActionResult> AddMember(Guid id, AddMemberRequest request) =>
        Map(await projects.AddMemberAsync(UserId, id, request));

    [HttpDelete("{id:guid}/members/{memberUserId:guid}")]
    public async Task<IActionResult> RemoveMember(Guid id, Guid memberUserId) =>
        Map(await projects.RemoveMemberAsync(UserId, id, memberUserId), _ => NoContent());

    private IActionResult Map<T>(ServiceResult<T> r, Func<T, IActionResult>? ok = null) => r.Status switch
    {
        ServiceStatus.Ok => ok is null ? Ok(r.Value) : ok(r.Value!),
        ServiceStatus.NotFound => NotFound(new { error = r.Error }),
        ServiceStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new { error = r.Error }),
        _ => BadRequest(new { error = r.Error })
    };
}
