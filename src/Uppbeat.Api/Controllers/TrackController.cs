using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Interfaces;

namespace Uppbeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly ITrackService _service;

    public TracksController(ITrackService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? genre = null, [FromQuery] string? search = null)
    {
        var tracks = await _service.GetAllAsync(page, pageSize, genre, search);
        return Ok(tracks);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var track = await _service.GetByIdAsync(id);
        if (track == null) return NotFound();
        return Ok(track);
    }

    [Authorize(Roles = "Artist,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TrackCreateDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var track = await _service.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = track.Id }, track);
    }

    [Authorize(Roles = "Artist,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TrackUpdateDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;

        try
        {
            var track = await _service.UpdateAsync(id, dto, userId, role);
            if (track == null) return NotFound();
            return Ok(track);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [Authorize(Roles = "Artist,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;

        try
        {
            var success = await _service.DeleteAsync(id, userId, role);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
