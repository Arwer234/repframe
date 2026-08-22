using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RepFrame.Api.Features.Exercises;
using RepFrame.Api.Models;
using Xunit;

namespace RepFrame.Api.Tests.Features.Exercises;

public class ExerciseHandlerTests
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
    public async Task CreateAsync_ShouldCreateExercise()
    {
        // Arrange
        var context = CreateContext();
        var handler = new ExerciseHandler(context);
        var request = new CreateExerciseRequest(
            Name: "Bench Press",
            MuscleGroup: "Chest"
        );

        // Act
        var result = await handler.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Bench Press");
        result.MuscleGroup.Should().Be("Chest");

        var dbExercise = context.Exercises.FirstOrDefault(e => e.Id == result.Id);
        dbExercise.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExercise()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();

        var existingExercise = new Exercise
        {
            Id = exerciseId,
            Name = "Old Name",
            MuscleGroup = "Old Group"
        };

        context.Exercises.Add(existingExercise);
        await context.SaveChangesAsync();

        var handler = new ExerciseHandler(context);
        var request = new UpdateExerciseRequest(
            Name: "New Name",
            MuscleGroup: "New Group"
        );

        // Act
        var result = await handler.UpdateAsync(exerciseId, request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New Name");
        result.MuscleGroup.Should().Be("New Group");

        var dbExercise = context.Exercises.First(e => e.Id == exerciseId);
        dbExercise.Name.Should().Be("New Name");
        dbExercise.MuscleGroup.Should().Be("New Group");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        var context = CreateContext();
        var handler = new ExerciseHandler(context);
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateExerciseRequest(
            Name: "Ghost",
            MuscleGroup: null
        );

        // Act
        var result = await handler.UpdateAsync(nonExistentId, request);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteExercise()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();

        var existingExercise = new Exercise
        {
            Id = exerciseId,
            Name = "To Delete",
            MuscleGroup = null
        };

        context.Exercises.Add(existingExercise);
        await context.SaveChangesAsync();

        var handler = new ExerciseHandler(context);

        // Act
        var result = await handler.DeleteAsync(exerciseId);

        // Assert
        result.Should().BeTrue();
        context.Exercises.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalseWhenNotFound()
    {
        // Arrange
        var context = CreateContext();
        var handler = new ExerciseHandler(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await handler.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnExercise()
    {
        // Arrange
        var context = CreateContext();
        var exerciseId = Guid.NewGuid();

        var existingExercise = new Exercise
        {
            Id = exerciseId,
            Name = "Squat",
            MuscleGroup = "Legs"
        };

        context.Exercises.Add(existingExercise);
        await context.SaveChangesAsync();

        var handler = new ExerciseHandler(context);

        // Act
        var result = await handler.GetByIdAsync(exerciseId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Squat");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenNotFound()
    {
        // Arrange
        var context = CreateContext();
        var handler = new ExerciseHandler(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await handler.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllExercises()
    {
        // Arrange
        var context = CreateContext();
        var handler = new ExerciseHandler(context);

        context.Exercises.AddRange(
            new Exercise { Id = Guid.NewGuid(), Name = "Bench Press", MuscleGroup = "Chest" },
            new Exercise { Id = Guid.NewGuid(), Name = "Squat", MuscleGroup = "Legs" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await handler.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }
}
