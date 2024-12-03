namespace MusicPlayerDB.Domain.DTOs;

public record SearchItemDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
