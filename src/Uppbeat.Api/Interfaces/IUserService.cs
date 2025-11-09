using Uppbeat.Api.DTOs;

namespace Uppbeat.Api.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserReadDto>> GetAllAsync();
    Task<UserReadDto?> GetByIdAsync(Guid id);
    Task<UserReadDto> CreateAsync(UserCreateDto dto);
    Task<UserReadDto?> UpdateAsync(Guid id, UserUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}