using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RepFrame.Api.Features.WorkoutSessions;
using RepFrame.Api.Models;
using Xunit;

namespace RepFrame.Api.Tests.Features.WorkoutSessions;

public class WorkoutSessionHandlerTests
{
    private static RepFrameDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

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
    public async Task CreateAsync_ShouldCreateSession()
    {
        // Arrange
        var context = CreateContext();
        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.CreateAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        var dbSession = context.WorkoutSessions.FirstOrDefault(s => s.FinishedAt == null);
        dbSession.Should().NotBeNull();
        dbSession!.StartedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task GetActiveAsync_ShouldReturnNullWhenNoActiveSession()
    {
        // Arrange
        var context = CreateContext();
        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.GetActiveAsync();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetActiveAsync_ShouldReturnActiveSession()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();
        var workoutSessionId = Guid.NewGuid();

        context.Exercises.Add(new Exercise { Id = exerciseId, Name = "Test Exercise" });

        var activeSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(activeSession);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.GetActiveAsync();

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(workoutSessionId);
    }

    [Fact]
    public async Task GetActiveWithSetsAsync_ShouldReturnSessionWithRelatedData()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();
        var workoutSessionId = Guid.NewGuid();

        context.Exercises.Add(new Exercise { Id = exerciseId, Name = "Bench Press" });

        var activeSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(activeSession);

        var set = new Set
        {
            Id = Guid.NewGuid(),
            WorkoutSessionId = workoutSessionId,
            ExerciseId = exerciseId,
            Number = 1,
            WeightKg = 80m,
            Reps = 5,
            Rir = 2,
            Type = SetType.Working,
            CreatedAt = DateTime.UtcNow
        };

        context.Sets.Add(set);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.GetActiveWithSetsAsync();

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(workoutSessionId);
        result.Sets.Should().ContainSingle();
        result.Sets.First().ExerciseId.Should().Be(exerciseId);
    }

    [Fact]
    public async Task GetActiveWithSetsAsync_ShouldReturnNullWhenNoActiveSession()
    {
        // Arrange
        var context = CreateContext();
        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.GetActiveWithSetsAsync();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task FinishAsync_ShouldMarkSessionAsInactive()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();
        var workoutSessionId = Guid.NewGuid();

        context.Exercises.Add(new Exercise { Id = exerciseId, Name = "Test Exercise" });

        var activeSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(activeSession);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.FinishAsync(workoutSessionId, null);

        // Assert
        result.Should().BeTrue();

        var dbSession = context.WorkoutSessions.Find(workoutSessionId);
        dbSession.Should().NotBeNull();
        dbSession!.FinishedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task FinishAsync_ShouldSetNote()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();
        var workoutSessionId = Guid.NewGuid();

        context.Exercises.Add(new Exercise { Id = exerciseId, Name = "Test Exercise" });

        var activeSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(activeSession);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);
        const string noteText = "Great session, felt strong";

        // Act
        var result = await handler.FinishAsync(workoutSessionId, noteText);

        // Assert
        result.Should().BeTrue();

        var dbSession = context.WorkoutSessions.Find(workoutSessionId);
        dbSession.Should().NotBeNull();
        dbSession!.Note.Should().Be(noteText);
    }

    [Fact]
    public async Task FinishAsync_ShouldReturnFalseWhenNotFound()
    {
        // Arrange
        var context = CreateContext();
        var handler = new WorkoutSessionHandler(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await handler.FinishAsync(nonExistentId, null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNote()
    {
        // Arrange
        var context = CreateContext();
        var workoutSessionId = Guid.NewGuid();

        var activeSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(activeSession);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);
        const string noteText = "Feeling strong today";

        // Act
        var result = await handler.UpdateAsync(workoutSessionId, noteText);

        // Assert
        result.Should().BeTrue();

        var dbSession = context.WorkoutSessions.Find(workoutSessionId);
        dbSession.Should().NotBeNull();
        dbSession!.Note.Should().Be(noteText);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalseWhenNotFound()
    {
        // Arrange
        var context = CreateContext();
        var handler = new WorkoutSessionHandler(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await handler.UpdateAsync(nonExistentId, "note");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalseWhenSessionAlreadyFinished()
    {
        // Arrange
        var context = CreateContext();
        var workoutSessionId = Guid.NewGuid();

        var finishedSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = DateTime.UtcNow.AddDays(-1),
            FinishedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(finishedSession);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.UpdateAsync(workoutSessionId, "note");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotChangeOtherProperties()
    {
        // Arrange
        var context = CreateContext();
        var workoutSessionId = Guid.NewGuid();
        var originalStartedAt = new DateTime(2026, 8, 22, 10, 0, 0);

        var activeSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = originalStartedAt
        };

        context.WorkoutSessions.Add(activeSession);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.UpdateAsync(workoutSessionId, "new note");

        // Assert
        result.Should().BeTrue();

        var dbSession = context.WorkoutSessions.Find(workoutSessionId);
        dbSession.Should().NotBeNull();
        dbSession!.StartedAt.Should().Be(originalStartedAt);
        dbSession.FinishedAt.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNullNote()
    {
        // Arrange
        var context = CreateContext();
        var workoutSessionId = Guid.NewGuid();

        var activeSession = new WorkoutSession
        {
            Id = workoutSessionId,
            StartedAt = DateTime.UtcNow,
            Note = "old note"
        };

        context.WorkoutSessions.Add(activeSession);
        await context.SaveChangesAsync();

        var handler = new WorkoutSessionHandler(context);

        // Act
        var result = await handler.UpdateAsync(workoutSessionId, null);

        // Assert
        result.Should().BeTrue();

        var dbSession = context.WorkoutSessions.Find(workoutSessionId);
        dbSession.Should().NotBeNull();
        dbSession!.Note.Should().Be("old note");
    }
}
