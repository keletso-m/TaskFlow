namespace TaskFlow.Core.Entities;

public class TaskDependency
{
    public Guid TaskId { get; set; }
    public ProjectTask Task { get; set; } = null!;
    public Guid DependsOnTaskId { get; set; }
    public ProjectTask DependsOnTask { get; set; } = null!;
}
