namespace Uppbeat.Api.DTOs;

public class TrackCreateDto
{
    public string Name { get; set; } = null!;
    public string ArtistId { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public int DurationSeconds { get; set; }
    public List<string> Genres { get; set; } = new();
}