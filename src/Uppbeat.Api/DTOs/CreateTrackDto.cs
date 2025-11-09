using System.ComponentModel.DataAnnotations;

namespace Uppbeat.Api.DTOs;

public class CreateTrackDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public Guid ArtistId { get; set; }
    [Range(1, 60*60*24)] // 1 second .. 1 day
    public int DurationSeconds { get; set; }
    [Required]
    public string FilePath { get; set; } = null!;
    [Required]
    [MinLength(1)]
    public List<string> Genres { get; set; } = new();
}