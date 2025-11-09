using Microsoft.AspNetCore.Identity;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Interfaces;
using Uppbeat.Api.Models;

namespace Uppbeat.Api.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new(); // Replace with EF DbContext later
    private readonly PasswordHasher<User> _passwordHasher = new();

    public Task<IEnumerable<UserReadDto>> GetAllAsync()
    {
        var result = _users.Select(u => new UserReadDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role
        });

        return Task.FromResult(result);
    }

    public Task<UserReadDto?> GetByIdAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return Task.FromResult<UserReadDto?>(null);

        var dto = new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
        return Task.FromResult<UserReadDto?>(dto);
    }

    public Task<UserReadDto> CreateAsync(UserCreateDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        _users.Add(user);

        return Task.FromResult(new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        });
    }

    public Task<UserReadDto?> UpdateAsync(Guid id, UserUpdateDto dto)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return Task.FromResult<UserReadDto?>(null);

        if (!string.IsNullOrWhiteSpace(dto.Name))
            user.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        return Task.FromResult<UserReadDto?>(new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        });
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return Task.FromResult(false);

        _users.Remove(user);
        return Task.FromResult(true);
    }
}