namespace MusicPlayerDB.Domain.DTOs;

public class ArtistPlaysCountDTO
{
    public int Id { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public int PlaysCount { get; set; }
}
