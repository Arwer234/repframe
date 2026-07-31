using Microsoft.AspNetCore.Mvc;

namespace RepFrame.Api.Features.WorkoutSessions;

[ApiController]
[Route("api/workout-sessions")]
public class WorkoutSessionController(WorkoutSessionHandler handler) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<WorkoutSessionDto>>> GetAll()
    {
        var results = await handler.GetAllAsync();
        return Ok(results);
    }

    [HttpGet("active")]
    public async Task<ActionResult<WorkoutSessionDto>> GetActive()
    {
        var result = await handler.GetActiveAsync();
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkoutSessionDto>> GetById(Guid id)
    {
        var result = await handler.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutSessionDto>> Create()
    {
        var result = await handler.CreateAsync();
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/finish")]
    public async Task<ActionResult> Finish(Guid id)
    {
        var result = await handler.FinishAsync(id);
        return result ? NoContent() : NotFound();
    }
}
