using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Interfaces;

namespace Uppbeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        try
        {
            var user = await _authService.RegisterAsync(dto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        var response = await _authService.LoginAsync(dto);
        if (response == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(response);
    }
}