using TaskFlow.Core.Common;

namespace TaskFlow.Core.Entities;

public class Project : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public Guid OwnerId { get; set; }

    public ICollection<ProjectMember> Members { get; set; } = [];
    public ICollection<ProjectTask> Tasks { get; set; } = [];
    public ICollection<TaskLabel> Labels { get; set; } = [];
}
