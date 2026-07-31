using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RepFrame.Api.Features.Sets;
using RepFrame.Api.Models;
using Xunit;

namespace RepFrame.Api.Tests.Features.Sets;

public class SetHandlerTests
{
    private RepFrameDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        // Disable FK enforcement for tests (InMemory doesn't enforce them either)
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "PRAGMA foreign_keys = OFF";
        cmd.ExecuteNonQuery();

        var options = new DbContextOptionsBuilder<RepFrameDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new RepFrameDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateSet()
    {
        // Arrange
        var context = CreateContext();
        var handler = new SetHandler(context);
        var request = new CreateSetRequest(
            WorkoutSessionId: Guid.NewGuid(),
            ExerciseId: Guid.NewGuid(),
            Number: 1,
            WeightKg: 100m,
            Reps: 5,
            Rir: 2,
            Type: "Working"
        );

        // Act
        var result = await handler.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.WeightKg.Should().Be(100m);
        result.Reps.Should().Be(5);
        result.Rir.Should().Be(2);
        result.Type.Should().Be("Working");

        var dbSet = context.Sets.FirstOrDefault(s => s.Id == result.Id);
        dbSet.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteSet()
    {
        // Arrange
        var context = CreateContext();
        var setId = Guid.NewGuid();
        var workoutSessionId = Guid.NewGuid();
        var exerciseId = Guid.NewGuid();

        var existingSet = new Set
        {
            Id = setId,
            WorkoutSessionId = workoutSessionId,
            ExerciseId = exerciseId,
            Number = 1,
            WeightKg = 60m,
            Reps = 10,
            Rir = null,
            Type = SetType.WarmUp,
            CreatedAt = DateTimeOffset.UtcNow
        };

        context.Sets.Add(existingSet);
        await context.SaveChangesAsync();

        var handler = new SetHandler(context);

        // Act
        var result = await handler.DeleteAsync(setId);

        // Assert
        result.Should().BeTrue();
        context.Sets.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalseWhenNotFound()
    {
        // Arrange
        var context = CreateContext();
        var handler = new SetHandler(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await handler.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSetWithIncludes()
    {
        // Arrange
        var context = CreateContext();
        var workoutSessionId = Guid.NewGuid();
        var exerciseId = Guid.NewGuid();

        context.WorkoutSessions.Add(new WorkoutSession { Id = workoutSessionId, StartedAt = DateTimeOffset.UtcNow });
        context.Exercises.Add(new Exercise { Id = exerciseId, Name = "Test Exercise" });

        var existingSet = new Set
        {
            Id = Guid.NewGuid(),
            WorkoutSessionId = workoutSessionId,
            ExerciseId = exerciseId,
            Number = 2,
            WeightKg = 90m,
            Reps = 6,
            Rir = 1,
            Type = SetType.Working,
            CreatedAt = DateTimeOffset.UtcNow
        };

        context.Sets.Add(existingSet);
        await context.SaveChangesAsync();

        var handler = new SetHandler(context);

        // Act
        var result = await handler.GetByIdAsync(existingSet.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(existingSet.Id);
        result.WeightKg.Should().Be(90m);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        var context = CreateContext();
        var handler = new SetHandler(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await handler.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSets()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();
        var workoutSessionId1 = Guid.NewGuid();
        var workoutSessionId2 = Guid.NewGuid();

        context.WorkoutSessions.Add(new WorkoutSession { Id = workoutSessionId1, StartedAt = DateTimeOffset.UtcNow });
        context.WorkoutSessions.Add(new WorkoutSession { Id = workoutSessionId2, StartedAt = DateTimeOffset.UtcNow });
        context.Exercises.Add(new Exercise { Id = exerciseId, Name = "Test Exercise" });

        context.Sets.AddRange(
            new Set { Id = Guid.NewGuid(), WorkoutSessionId = workoutSessionId1, ExerciseId = exerciseId, Number = 1, WeightKg = 80m, Reps = 5, Rir = 2, Type = SetType.Working, CreatedAt = DateTimeOffset.UtcNow },
            new Set { Id = Guid.NewGuid(), WorkoutSessionId = workoutSessionId2, ExerciseId = exerciseId, Number = 1, WeightKg = 90m, Reps = 3, Rir = 0, Type = SetType.TopSet, CreatedAt = DateTimeOffset.UtcNow }
        );
        await context.SaveChangesAsync();

        var handler = new SetHandler(context);

        // Act
        var result = await handler.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }
}
