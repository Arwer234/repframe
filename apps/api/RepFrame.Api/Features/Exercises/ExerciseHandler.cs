using Microsoft.EntityFrameworkCore;
using RepFrame.Api.Mappers;

namespace RepFrame.Api.Features.Exercises;

public class ExerciseHandler(RepFrameDbContext context)
{
    public async Task<List<ExerciseDto>> GetAllAsync()
    {
        var exercises = await context.Exercises.ToListAsync();
        return [.. exercises.Select(ExerciseMapper.ToDto)];
    }

    public async Task<ExerciseDto?> GetByIdAsync(Guid id)
    {
        var exercise = await context.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        return exercise == null ? null : ExerciseMapper.ToDto(exercise);
    }

    public async Task<ExerciseDto> CreateAsync(CreateExerciseRequest request)
    {
        var exercise = ExerciseMapper.ToEntity(request);

        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        return ExerciseMapper.ToDto(exercise);
    }
}
