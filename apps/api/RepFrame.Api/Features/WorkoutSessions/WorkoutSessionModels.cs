namespace RepFrame.Api.Features.WorkoutSessions;

public record WorkoutSessionDto(
    Guid Id,
    DateTimeOffset StartedAt,
    DateTimeOffset? FinishedAt
);

public record CreateWorkoutSessionRequest();
