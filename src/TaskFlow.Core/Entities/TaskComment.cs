using TaskFlow.Core.Common;

namespace TaskFlow.Core.Entities;

public class TaskComment : BaseEntity
{
    public Guid TaskId { get; set; }
    public ProjectTask Task { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public required string Body { get; set; }
}
