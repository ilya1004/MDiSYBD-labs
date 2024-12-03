namespace MusicPlayerDB.Domain.DTOs;

public class FavouriteSongStats
{
    public int SongId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public int PlaysCount { get; set; }
    public int FavouritesCount { get; set; }
}
