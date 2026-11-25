using TaskFlow.Core.Common;

namespace TaskFlow.Core.Entities;

public class TaskLabel : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public required string Name { get; set; }
    public string Color { get; set; } = "#6B7280";

    public ICollection<ProjectTask> Tasks { get; set; } = [];
}
