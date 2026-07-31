using Riok.Mapperly.Abstractions;
using RepFrame.Api.Features.Exercises;
using RepFrame.Api.Models;

namespace RepFrame.Api.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class ExerciseMapper
{
  public static partial ExerciseDto ToDto(Exercise exercise);

  public static partial Exercise ToEntity(CreateExerciseRequest request);
}
