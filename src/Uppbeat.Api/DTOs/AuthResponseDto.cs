namespace Uppbeat.Api.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public UserReadDto User { get; set; } = null!;
}