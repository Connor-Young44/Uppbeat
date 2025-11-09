using Uppbeat.Api.DTOs;

namespace Uppbeat.Api.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
    Task<UserReadDto> RegisterAsync(RegisterRequestDto dto);
}