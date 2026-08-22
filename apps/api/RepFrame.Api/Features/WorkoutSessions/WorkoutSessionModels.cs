using RepFrame.Api.Features.Sets;

namespace RepFrame.Api.Features.WorkoutSessions;

public record WorkoutSessionDto(
    Guid Id,
    DateTime StartedAt,
    DateTime? FinishedAt,
    string? Note
);

public record FinishWorkoutRequest(
    string? Note = null
);

public record WorkoutSessionWithSetsDto(
    Guid Id,
    DateTime StartedAt,
    DateTime? FinishedAt,
    string? Note,
    List<SetDto> Sets
);

public record CreateWorkoutSessionRequest();
