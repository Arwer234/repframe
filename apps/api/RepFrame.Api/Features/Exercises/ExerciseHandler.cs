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

    public async Task<ExerciseDto?> UpdateAsync(Guid id, UpdateExerciseRequest request)
    {
        var exercise = await context.Exercises.FindAsync(id);
        if (exercise == null)
            return null;

        exercise.Name = request.Name;
        exercise.MuscleGroup = request.MuscleGroup;

        await context.SaveChangesAsync();

        return ExerciseMapper.ToDto(exercise);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exercise = await context.Exercises.FindAsync(id);
        if (exercise == null)
            return false;

        context.Exercises.Remove(exercise);
        await context.SaveChangesAsync();
        return true;
    }
}
