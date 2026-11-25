using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Infrastructure.Identity;

namespace TaskFlow.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
    public DbSet<TaskComment> TaskComments => Set<TaskComment>();
    public DbSet<TaskLabel> TaskLabels => Set<TaskLabel>();
    public DbSet<TaskActivity> TaskActivities => Set<TaskActivity>();
    public DbSet<TaskDependency> TaskDependencies => Set<TaskDependency>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
