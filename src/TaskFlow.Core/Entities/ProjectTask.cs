using TaskFlow.Core.Common;
using TaskFlow.Core.Enums;

namespace TaskFlow.Core.Entities;

public class ProjectTask : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskType Type { get; set; } = TaskType.Feature;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;

    public Guid ReporterId { get; set; }
    public Guid? AssigneeId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid? ParentTaskId { get; set; }
    public ProjectTask? ParentTask { get; set; }
    public ICollection<ProjectTask> Subtasks { get; set; } = [];

    public ICollection<TaskComment> Comments { get; set; } = [];
    public ICollection<TaskLabel> Labels { get; set; } = [];
    public ICollection<TaskActivity> Activities { get; set; } = [];
    public ICollection<TaskDependency> Dependencies { get; set; } = [];  // tasks this one depends on
    public ICollection<TaskDependency> Dependents { get; set; } = [];    // tasks that depend on this one
}
