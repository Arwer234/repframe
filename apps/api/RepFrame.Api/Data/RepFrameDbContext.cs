using Microsoft.EntityFrameworkCore;
using RepFrame.Api.Models;

namespace RepFrame.Api;

public class RepFrameDbContext : DbContext
{
    public RepFrameDbContext(DbContextOptions<RepFrameDbContext> options) : base(options) { }

    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Set> Sets => Set<Set>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkoutSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartedAt).IsRequired();
            entity.HasMany(e => e.Sets).WithOne(e => e.WorkoutSession).HasForeignKey(e => e.WorkoutSessionId);
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasMany(e => e.Sets).WithOne(e => e.Exercise).HasForeignKey(e => e.ExerciseId);
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Number).IsRequired();
            entity.Property(e => e.WeightKg).IsRequired();
            entity.Property(e => e.Reps).IsRequired();
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.WorkoutSessionId);
            entity.HasIndex(e => e.ExerciseId);
        });
    }
}
