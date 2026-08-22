using Microsoft.EntityFrameworkCore;
using RepFrame.Api;
using RepFrame.Api.Mappers;
using RepFrame.Api.Models;

namespace RepFrame.Api.Features.Sets;

public class SetHandler(RepFrameDbContext context)
{
    public async Task<SetDto> CreateAsync(CreateSetRequest request)
    {
        var set = SetMapper.ToEntity(request);

        context.Sets.Add(set);
        await context.SaveChangesAsync();

        return SetMapper.ToDto(set);
    }

    public async Task<SetDto?> GetByIdAsync(Guid id)
    {
        var set = await context.Sets
            .Include(s => s.WorkoutSession)
            .Include(s => s.Exercise)
            .FirstOrDefaultAsync(s => s.Id == id);

        return set == null ? null : SetMapper.ToDto(set);
    }

    public async Task<List<SetDto>> GetAllAsync()
    {
        var sets = await context.Sets
            .Include(s => s.WorkoutSession)
            .Include(s => s.Exercise)
            .ToListAsync();

        return sets.Select(SetMapper.ToDto).ToList();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var set = await context.Sets.FindAsync(id);
        if (set == null)
            return false;

        context.Sets.Remove(set);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<SetDto?> UpdateAsync(Guid id, UpdateSetRequest request)
    {
        var set = await context.Sets.FindAsync(id);
        if (set == null)
            return null;

        set.Number = request.Number;
        set.WeightKg = request.WeightKg;
        set.Reps = request.Reps;
        set.Rir = request.Rir;
        set.Type = Enum.Parse<SetType>(request.Type);

        await context.SaveChangesAsync();

        return SetMapper.ToDto(set);
    }

    public async Task<SetDto> CopyPreviousSetAsync(Guid exerciseId, Guid workoutSessionId)
    {
        var previousSet = await context.Sets
            .Where(s => s.ExerciseId == exerciseId)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        if (previousSet == null)
            throw new InvalidOperationException("No previous set found for this exercise.");

        var newSet = new Set
        {
            WorkoutSessionId = workoutSessionId,
            ExerciseId = exerciseId,
            Number = previousSet.Number + 1,
            WeightKg = previousSet.WeightKg,
            Reps = previousSet.Reps,
            Rir = previousSet.Rir,
            Type = previousSet.Type,
            CreatedAt = DateTime.UtcNow
        };

        context.Sets.Add(newSet);
        await context.SaveChangesAsync();

        return SetMapper.ToDto(newSet);
    }

    public async Task<LastResultDto?> GetLastResultForExerciseAsync(Guid exerciseId)
    {
        var lastSet = await context.Sets
            .Where(s => s.ExerciseId == exerciseId)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastSet == null)
            return null;

        return new LastResultDto(
            lastSet.Id,
            lastSet.Number,
            lastSet.WeightKg,
            lastSet.Reps,
            lastSet.Rir,
            lastSet.Type.ToString(),
            lastSet.CreatedAt
        );
    }
}
