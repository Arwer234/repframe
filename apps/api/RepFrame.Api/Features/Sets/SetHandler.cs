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
}
