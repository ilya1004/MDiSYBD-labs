namespace MusicPlayerDB.Domain.DTOs;

public  record SongByPlaylistDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationSecs { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public string PlaylistName { get; set; } = string.Empty;
}
