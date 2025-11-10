namespace Uppbeat.Api.Models;

public class Track
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public Guid ArtistId { get; set; }
    public User? Owner { get; set; }
    public int DurationSeconds { get; set; }
    public string FilePath { get; set; } = null!;
    public List<TrackGenre> Genres { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}