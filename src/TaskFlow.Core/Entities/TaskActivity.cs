using TaskFlow.Core.Common;

namespace TaskFlow.Core.Entities;

public class TaskActivity : BaseEntity
{
    public Guid TaskId { get; set; }
    public ProjectTask Task { get; set; } = null!;
    public Guid ActorId { get; set; }
    public required string Action { get; set; }   // e.g. "StatusChanged"
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
