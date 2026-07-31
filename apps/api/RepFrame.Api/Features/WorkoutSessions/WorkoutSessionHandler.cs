using Microsoft.EntityFrameworkCore;
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
            StartedAt = DateTimeOffset.UtcNow
        };

        context.WorkoutSessions.Add(session);
        await context.SaveChangesAsync();

        return WorkoutSessionMapper.ToDto(session);
    }

    public async Task<bool> FinishAsync(Guid id)
    {
        var session = await context.WorkoutSessions.FindAsync(id);
        if (session == null)
            return false;

        session.FinishedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync();

        return true;
    }
}
