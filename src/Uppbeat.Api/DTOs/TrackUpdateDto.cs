using System.ComponentModel.DataAnnotations;

namespace Uppbeat.Api.DTOs;

public class TrackUpdateDto
{
    public string? Name { get; set; }
    public string? Artist { get; set; }
    public int? DurationSeconds { get; set; }
    public string FilePath { get; set; } = null!;
    public List<string>? Genres { get; set; }
}