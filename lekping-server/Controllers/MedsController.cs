using lekping.server.Features.Meds.Dtos;
using lekping.server.Features.Meds.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace lekping.server.Controllers;
[ApiController]
[Route("api/v1/meds")]
[Authorize]
public sealed class MedsController : ControllerBase
{
    private readonly IMedsService _svc;
    public MedsController(IMedsService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MedDto>>> GetAll(
        [FromQuery] int skip = 0, [FromQuery] int take = 50, CancellationToken ct = default)
        => Ok(await _svc.GetAllAsync(skip, take, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MedDto>> GetById(Guid id, CancellationToken ct = default)
    {
        var med = await _svc.FindAsync(id, ct);
        return med is null ? NotFound() : Ok(med);
    }

    [HttpPost]
    public async Task<ActionResult<MedDto>> Create([FromBody] CreateMedRequest req, CancellationToken ct = default)
    {
        var created = await _svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MedDto>> Update(Guid id, [FromBody] UpdateMedRequest req, CancellationToken ct = default)
    {
        if (id != req.Id) return BadRequest("Route id and body id differ.");
        var updated = await _svc.UpdateAsync(req, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var removed = await _svc.DeleteAsync(id, ct);
        return removed ? NoContent() : NotFound();
    }
}
