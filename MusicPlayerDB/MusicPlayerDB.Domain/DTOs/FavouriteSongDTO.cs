namespace MusicPlayerDB.Domain.DTOs;

public class FavouriteSongDTO
{
    public int Id { get; set; }
    public DateOnly DateAdded { get; set; }
    public int SongId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationSecs { get; set; }
    public string ArtistName { get; set; } = string.Empty;
}
