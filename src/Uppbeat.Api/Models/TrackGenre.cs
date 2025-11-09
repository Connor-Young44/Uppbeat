namespace Uppbeat.Api.Models;

public class TrackGenre
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TrackId { get; set; }
    public Track Track { get; set; } = null!;
    public string Name { get; set; } = null!;
}