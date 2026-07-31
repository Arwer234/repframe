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
}
