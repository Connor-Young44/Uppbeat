using Microsoft.EntityFrameworkCore;
using Uppbeat.Api.Data;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Interfaces;
using Uppbeat.Api.Models;

namespace Uppbeat.Api.Services;

public class TrackService : ITrackService
{
    private readonly AppDbContext _db;

    public TrackService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TrackReadDto> CreateAsync(TrackCreateDto dto, Guid ownerId)
    {
        var track = new Track
        {
            Name = dto.Name,
            ArtistId = ownerId,
            DurationSeconds = dto.DurationSeconds,
            FilePath = dto.FilePath,
            Genres = dto.Genres.Select(g => new TrackGenre { Name = g }).ToList(),
            CreatedAt = DateTime.UtcNow
        };

        _db.Tracks.Add(track);
        await _db.SaveChangesAsync();

        return MapToDto(track);
    }

    public async Task<TrackReadDto?> GetByIdAsync(Guid id)
    {
        var track = await _db.Tracks
            .Include(t => t.Genres)
            .Include(t => t.ArtistId)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (track == null) return null;
        return MapToDto(track);
    }

    public async Task<IEnumerable<TrackReadDto>> GetAllAsync(
        int page = 1,
        int pageSize = 20,
        string? genre = null,
        string? search = null)
    {
        var query = _db.Tracks
            .Include(t => t.Genres)
            .Include(t => t.Owner)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(t => t.Genres.Any(g => g.Name.ToLower() == genre.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(t => t.Name.ToLower().Contains(searchLower)
                                     || t.Owner!.Name.ToLower().Contains(searchLower));
        }

        query = query
            .OrderBy(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var tracks = await query.ToListAsync();
        return tracks.Select(MapToDto);
    }

    public async Task<TrackReadDto?> UpdateAsync(Guid id, TrackUpdateDto dto, Guid requesterId, string requesterRole)
    {
        var track = await _db.Tracks
            .Include(t => t.Genres)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (track == null)
            return null;

        if (requesterRole == "Artist" && track.ArtistId != requesterId && requesterRole != "Admin")
            throw new UnauthorizedAccessException("Not allowed to modify this track.");

        if (!string.IsNullOrWhiteSpace(dto.Name))
            track.Name = dto.Name;

        if (dto.DurationSeconds.HasValue)
            track.DurationSeconds = dto.DurationSeconds.Value;

        if (!string.IsNullOrWhiteSpace(dto.FilePath))
            track.FilePath = dto.FilePath;

        if (dto.Genres != null)
        {
            _db.TrackGenres.RemoveRange(track.Genres);
            await _db.SaveChangesAsync(); // <-- finalize deletion before adding new ones

            track.Genres = dto.Genres.Select(g => new TrackGenre
            {
                Name = g,
                TrackId = track.Id
            }).ToList();
            _db.TrackGenres.AddRange(track.Genres);
        }

        await _db.SaveChangesAsync();
        return MapToDto(track);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid requesterId, string requesterRole)
    {
        var track = await _db.Tracks.FirstOrDefaultAsync(t => t.Id == id);
        if (track == null) return false;

        if ((requesterRole == "Artist" && track.ArtistId != requesterId) && requesterRole != "Admin")
            throw new UnauthorizedAccessException("Not allowed to delete this track.");

        _db.Tracks.Remove(track);
        await _db.SaveChangesAsync();
        return true;
    }

    private static TrackReadDto MapToDto(Track track)
    {
        return new TrackReadDto
        {
            Id = track.Id,
            Name = track.Name,
            ArtistId = track.ArtistId,
            DurationSeconds = track.DurationSeconds,
            FilePath = track.FilePath,
            Genres = track.Genres.Select(g => g.Name).ToList(),
        };
    }
}
