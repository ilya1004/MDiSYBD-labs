namespace MusicPlayerDB.Domain.Entities;

public class ArtistInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateOnly Birthday { get; set; }
}
