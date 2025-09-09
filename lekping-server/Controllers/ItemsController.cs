using LekPing.Server.Features.Items.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LekPing.Server.Features.Items;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsService _svc;

    public ItemsController(IItemsService svc) => _svc = svc;

    // GET /api/items?skip=0&take=50
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ItemDto>>> GetAll(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50,
        CancellationToken ct = default)
    {
        if (take is < 1 or > 200) take = 50;
        var items = await _svc.GetAllAsync(skip, take, ct);
        return Ok(items);
    }

    // GET /api/items/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var item = await _svc.FindAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST /api/items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create(
        [FromBody] CreateItemRequest req,
        CancellationToken ct = default)
    {
        var created = await _svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/items/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ItemDto>> Update(
        Guid id,
        [FromBody] UpdateItemRequest req,
        CancellationToken ct = default)
    {
        if (id != req.Id) return BadRequest("Route id and body id differ.");
        var updated = await _svc.UpdateAsync(req, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    // DELETE /api/items/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var removed = await _svc.DeleteAsync(id, ct);
        return removed ? NoContent() : NotFound();
    }
}
