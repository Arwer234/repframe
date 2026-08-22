using Microsoft.AspNetCore.Mvc;

namespace RepFrame.Api.Features.Sets;

[ApiController]
[Route("api/sets")]
public class SetController(SetHandler handler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SetDto>> Create([FromBody] CreateSetRequest request)
    {
        var result = await handler.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SetDto>> GetById(Guid id)
    {
        var result = await handler.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<SetDto>>> GetAll()
    {
        var results = await handler.GetAllAsync();
        return Ok(results);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await handler.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SetDto>> Update(Guid id, [FromBody] UpdateSetRequest request)
    {
        var result = await handler.UpdateAsync(id, request);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("exercise/{exerciseId:guid}/copy-previous")]
    public async Task<ActionResult<SetDto>> CopyPrevious(Guid exerciseId, Guid workoutSessionId)
    {
        try
        {
            var result = await handler.CopyPreviousSetAsync(exerciseId, workoutSessionId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpGet("exercise/{exerciseId:guid}/last-result")]
    public async Task<ActionResult<LastResultDto>> GetLastResult(Guid exerciseId)
    {
        var result = await handler.GetLastResultForExerciseAsync(exerciseId);
        return result == null ? NotFound() : Ok(result);
    }
}
