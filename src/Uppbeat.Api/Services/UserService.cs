using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uppbeat.Api.Data;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Interfaces;
using Uppbeat.Api.Models;

namespace Uppbeat.Api.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UserReadDto>> GetAllAsync()
    {
        return await _db.Users
            .Select(u => new UserReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            })
            .ToListAsync();
    }

    public async Task<UserReadDto?> GetByIdAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return null;

        return new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<UserReadDto?> UpdateAsync(Guid id, UserUpdateDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return null;

        if (!string.IsNullOrWhiteSpace(dto.Name))
            user.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _db.SaveChangesAsync();

        return new UserReadDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}
