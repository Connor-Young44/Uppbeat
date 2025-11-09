using System.ComponentModel.DataAnnotations;

namespace Uppbeat.Api.DTOs;

public class UpdateTrackDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Range(1, 60*60*24)]
    public int DurationSeconds { get; set; }
    [Required]
    public List<string> Genres { get; set; } = new();
}