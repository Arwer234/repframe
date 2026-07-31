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
}
