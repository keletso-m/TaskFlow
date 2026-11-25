using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Core.Entities;
using TaskFlow.Infrastructure.Identity;

namespace TaskFlow.Infrastructure.Persistence;

internal class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> b)
    {
        b.Property(p => p.Name).HasMaxLength(200).IsRequired();
        b.Property(p => p.Description).HasMaxLength(2000);
        b.HasOne<ApplicationUser>().WithMany()
            .HasForeignKey(p => p.OwnerId).OnDelete(DeleteBehavior.Restrict);
    }
}

internal class ProjectMemberConfig : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> b)
    {
        b.HasKey(m => new { m.ProjectId, m.UserId });
        b.Property(m => m.Role).HasConversion<string>().HasMaxLength(20);
        b.HasOne(m => m.Project).WithMany(p => p.Members)
            .HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne<ApplicationUser>().WithMany()
            .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class ProjectTaskConfig : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> b)
    {
        b.ToTable("Tasks");
        b.Property(t => t.Title).HasMaxLength(300).IsRequired();
        b.Property(t => t.Description).HasMaxLength(10000);
        b.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);
        b.Property(t => t.Priority).HasConversion<string>().HasMaxLength(20);
        b.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);

        b.HasOne(t => t.Project).WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne<ApplicationUser>().WithMany()
            .HasForeignKey(t => t.ReporterId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<ApplicationUser>().WithMany()
            .HasForeignKey(t => t.AssigneeId).OnDelete(DeleteBehavior.SetNull);
        b.HasOne(t => t.ParentTask).WithMany(t => t.Subtasks)
            .HasForeignKey(t => t.ParentTaskId).OnDelete(DeleteBehavior.Cascade);

        b.HasMany(t => t.Labels).WithMany(l => l.Tasks)
            .UsingEntity(j => j.ToTable("TaskLabelAssignments"));

        b.HasIndex(t => new { t.ProjectId, t.Status });
        b.HasIndex(t => t.AssigneeId);
    }
}

internal class TaskCommentConfig : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> b)
    {
        b.Property(c => c.Body).HasMaxLength(4000).IsRequired();
        b.HasOne(c => c.Task).WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne<ApplicationUser>().WithMany()
            .HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.Restrict);
    }
}

internal class TaskLabelConfig : IEntityTypeConfiguration<TaskLabel>
{
    public void Configure(EntityTypeBuilder<TaskLabel> b)
    {
        b.Property(l => l.Name).HasMaxLength(50).IsRequired();
        b.Property(l => l.Color).HasMaxLength(7);
        b.HasOne(l => l.Project).WithMany(p => p.Labels)
            .HasForeignKey(l => l.ProjectId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(l => new { l.ProjectId, l.Name }).IsUnique();
    }
}

internal class TaskActivityConfig : IEntityTypeConfiguration<TaskActivity>
{
    public void Configure(EntityTypeBuilder<TaskActivity> b)
    {
        b.Property(a => a.Action).HasMaxLength(100).IsRequired();
        b.Property(a => a.OldValue).HasMaxLength(1000);
        b.Property(a => a.NewValue).HasMaxLength(1000);
        b.HasOne(a => a.Task).WithMany(t => t.Activities)
            .HasForeignKey(a => a.TaskId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne<ApplicationUser>().WithMany()
            .HasForeignKey(a => a.ActorId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(a => new { a.TaskId, a.CreatedAt });
    }
}

internal class TaskDependencyConfig : IEntityTypeConfiguration<TaskDependency>
{
    public void Configure(EntityTypeBuilder<TaskDependency> b)
    {
        b.HasKey(d => new { d.TaskId, d.DependsOnTaskId });
        b.HasOne(d => d.Task).WithMany(t => t.Dependencies)
            .HasForeignKey(d => d.TaskId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(d => d.DependsOnTask).WithMany(t => t.Dependents)
            .HasForeignKey(d => d.DependsOnTaskId).OnDelete(DeleteBehavior.Cascade);
    }
}
