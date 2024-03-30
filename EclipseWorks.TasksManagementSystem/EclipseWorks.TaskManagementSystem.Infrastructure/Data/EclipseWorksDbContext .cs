using EclipseWorks.TaskManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.TaskManagementSystem.Infrastructure.Data;
public class EclipseWorksDbContext : DbContext
{
    public EclipseWorksDbContext(DbContextOptions<EclipseWorksDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserAccount> UserAccount { get; set; } = null!;
    public DbSet<Project> Project { get; set; } = null!;
    public DbSet<ProjectTask> ProjectTask { get; set; } = null!;
    public DbSet<ProjectTaskComment> ProjectTaskComment { get; set; } = null!;
    public DbSet<ProjectTaskHistory> ProjectTaskHistory { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectTask>()
            .HasMany(pt => pt.ProjectTaskComment)
            .WithOne(c => c.ProjectTask)
            .HasForeignKey(c => c.ProjectTaskId);

        modelBuilder.Entity<ProjectTask>()
            .HasMany(pt => pt.ProjectTaskHistory)
            .WithOne(h => h.ProjectTask)
            .HasForeignKey(h => h.ProjectTaskId);

        modelBuilder.Entity<ProjectTaskHistory>()
            .HasOne(h => h.ProjectTaskComment)
            .WithOne()
            .HasForeignKey<ProjectTaskHistory>(h => h.ProjectTaskCommentId);

        modelBuilder.Entity<ProjectTask>()
            .HasOne(pt => pt.AssignedToUserAccount)
            .WithMany() 
            .HasForeignKey(pt => pt.AssignedToUserAccountId)
            .IsRequired();

    }
}
