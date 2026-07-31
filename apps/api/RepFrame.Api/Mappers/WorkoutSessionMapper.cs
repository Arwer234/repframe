using Riok.Mapperly.Abstractions;
using RepFrame.Api.Features.WorkoutSessions;
using RepFrame.Api.Models;

namespace RepFrame.Api.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class WorkoutSessionMapper
{
    public static partial WorkoutSessionDto ToDto(WorkoutSession session);
}
