using Uppbeat.Api.DTOs;

namespace Uppbeat.Api.Interfaces;

public interface ITrackService
{
    Task<TrackReadDto> CreateAsync(TrackCreateDto dto, Guid ownerId);
    Task<TrackReadDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TrackReadDto>> GetAllAsync(int page = 1, int pageSize = 20, string? genre = null, string? search = null);
    Task<TrackReadDto?> UpdateAsync(Guid id, TrackUpdateDto dto, Guid requesterId, string requesterRole);
    Task<bool> DeleteAsync(Guid id, Guid requesterId, string requesterRole);
}