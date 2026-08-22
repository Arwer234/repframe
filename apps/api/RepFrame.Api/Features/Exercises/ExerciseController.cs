using Microsoft.AspNetCore.Mvc;

namespace RepFrame.Api.Features.Exercises;

[ApiController]
[Route("api/exercises")]
public class ExerciseController(ExerciseHandler handler) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ExerciseDto>>> GetAll()
    {
        var results = await handler.GetAllAsync();
        return Ok(results);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseDto>> GetById(Guid id)
    {
        var result = await handler.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseDto>> Create([FromBody] CreateExerciseRequest request)
    {
        var result = await handler.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ExerciseDto>> Update(Guid id, [FromBody] UpdateExerciseRequest request)
    {
        var result = await handler.UpdateAsync(id, request);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await handler.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
