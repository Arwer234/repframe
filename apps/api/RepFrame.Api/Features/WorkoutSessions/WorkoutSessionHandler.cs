using Microsoft.EntityFrameworkCore;
using RepFrame.Api.Features.Sets;
using RepFrame.Api.Mappers;
using RepFrame.Api.Models;

namespace RepFrame.Api.Features.WorkoutSessions;

public class WorkoutSessionHandler(RepFrameDbContext context)
{
    public async Task<List<WorkoutSessionDto>> GetAllAsync()
    {
        var sessions = await context.WorkoutSessions.ToListAsync();

        return [.. sessions.Select(WorkoutSessionMapper.ToDto)];
    }

    public async Task<WorkoutSessionDto?> GetByIdAsync(Guid id)
    {
        var session = await context.WorkoutSessions.FirstOrDefaultAsync(s => s.Id == id);
        
        return session == null ? null : WorkoutSessionMapper.ToDto(session);
    }

    public async Task<WorkoutSessionDto?> GetActiveAsync()
    {
        var session = await context.WorkoutSessions
            .Where(s => s.FinishedAt == null)
            .OrderByDescending(s => s.StartedAt)
            .FirstOrDefaultAsync();

        return session == null ? null : WorkoutSessionMapper.ToDto(session);
    }

    public async Task<WorkoutSessionDto> CreateAsync()
    {
        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            StartedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(session);
        await context.SaveChangesAsync();

        return WorkoutSessionMapper.ToDto(session);
    }

    public async Task<bool> FinishAsync(Guid id, string? note)
    {
        var session = await context.WorkoutSessions.FindAsync(id);
        if (session == null)
            return false;

        session.FinishedAt = DateTime.UtcNow;
        session.Note = note;
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, string? note)
    {
        var session = await context.WorkoutSessions.FindAsync(id);
        if (session == null || session.FinishedAt != null)
            return false;

        if (note is not null)
            session.Note = note;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<WorkoutSessionWithSetsDto?> GetActiveWithSetsAsync()
    {
        var session = await context.WorkoutSessions
            .Include(s => s.Sets)
                .ThenInclude(set => set.Exercise)
            .Where(s => s.FinishedAt == null)
            .OrderByDescending(s => s.StartedAt)
            .FirstOrDefaultAsync();

        if (session == null)
            return null;

        var sets = session.Sets.Select(set => new SetDto(
            set.Id,
            set.WorkoutSessionId,
            set.ExerciseId,
            set.Number,
            set.WeightKg,
            set.Reps,
            set.Rir,
            set.Type.ToString(),
            set.CreatedAt
        )).ToList();

        return new WorkoutSessionWithSetsDto(
            session.Id,
            session.StartedAt,
            session.FinishedAt,
            session.Note,
            sets
        );
    }
}
