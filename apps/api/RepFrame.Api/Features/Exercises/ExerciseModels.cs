namespace RepFrame.Api.Features.Exercises;

public record ExerciseDto(
    Guid Id,
    string Name,
    string? MuscleGroup
);

public record CreateExerciseRequest(
    string Name,
    string? MuscleGroup
);
