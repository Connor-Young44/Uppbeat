using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Interfaces;

namespace Uppbeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
    {
        var users = await userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<UserReadDto>> GetById(Guid id)
    {
        var user = await userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserReadDto>> Create(UserCreateDto dto)
    {
        var created = await userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<UserReadDto>> Update(Guid id, UserUpdateDto dto)
    {
        var updated = await userService.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await userService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}