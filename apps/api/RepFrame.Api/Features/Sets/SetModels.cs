namespace RepFrame.Api.Features.Sets;

public record CreateSetRequest(
    Guid WorkoutSessionId,
    Guid ExerciseId,
    int Number,
    decimal WeightKg,
    int Reps,
    int? Rir,
    string Type
);

public record UpdateSetRequest(
    int Number,
    decimal WeightKg,
    int Reps,
    int? Rir,
    string Type
);

public record SetDto(
    Guid Id,
    Guid WorkoutSessionId,
    Guid ExerciseId,
    int Number,
    decimal WeightKg,
    int Reps,
    int? Rir,
    string Type,
    DateTime CreatedAt
);

public record LastResultDto(
    Guid Id,
    int Number,
    decimal WeightKg,
    int Reps,
    int? Rir,
    string Type,
    DateTime CreatedAt
);
