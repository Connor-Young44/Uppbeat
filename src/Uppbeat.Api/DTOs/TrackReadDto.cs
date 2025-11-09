namespace Uppbeat.Api.DTOs;

public class TrackReadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid ArtistId { get; set; }
    public int DurationSeconds { get; set; }
    public string FilePath { get; set; } = null!;
    public List<string> Genres { get; set; } = new();
}