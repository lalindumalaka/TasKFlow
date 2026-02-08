//TaskFlow_net\TaskFlow.Data\TaskFlowDbContext.cs

using Microsoft.EntityFrameworkCore;
using TaskFlow.Shared.Entities;

namespace TaskFlow.Data;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TimeEntry> TimeEntries { get; set; }
    public DbSet<Status> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasOne(e => e.Status)
                .WithMany(s => s.TaskItems)
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TimeEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartTime)
                .IsRequired();
            entity.Property(e => e.EndTime)
                .IsRequired();
            entity.Property(e => e.Duration)
                .IsRequired();

            entity.HasOne(e => e.TaskItem)
                .WithMany(t => t.TimeEntries)
                .HasForeignKey(e => e.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        
        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Name)
                .IsUnique();
        });

        
        modelBuilder.Entity<Status>().HasData(
            new Status { Id = 1, Name = "To Do" },
            new Status { Id = 2, Name = "In Progress" },
            new Status { Id = 3, Name = "Review" },
            new Status { Id = 4, Name = "Completed" }
        );
    }
}

