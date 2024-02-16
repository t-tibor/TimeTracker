using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace TimeTracker.Models;

// dotnet ef dbcontext scaffold "Data Source=localhost;Initial Catalog=TimeTracker;Integrated Security=True;Trust Server Certificate=True"  Microsoft.EntityFrameworkCore.SqlServer -o Models

public partial class TimeTrackerContext : DbContext
{
    public TimeTrackerContext()
    {
    }

    public TimeTrackerContext(DbContextOptions<TimeTrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<WorkLog> WorkLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["TimeTrackerDb"].ConnectionString);
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.HasIndex(e => e.ProjectName, "UNIQ_ProjectName").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProjectName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("Task");

            entity.HasIndex(e => e.TaskName, "UNIQ_TaskName").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.TaskName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_ProjectID")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkLog>(entity =>
        {
            entity.ToTable("WorkLog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Summary)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaskId).HasColumnName("TaskID");

            entity.HasOne(d => d.Task).WithMany(p => p.WorkLogs)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_TaskID")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
