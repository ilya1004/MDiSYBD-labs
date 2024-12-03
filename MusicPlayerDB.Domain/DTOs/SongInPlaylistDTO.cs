namespace MusicPlayerDB.Domain.DTOs;

public record SongInPlaylistDTO
{
    public int SongId { get; set; }
    public string SongTitle { get; set; } = string.Empty;
    public int PlaylistId { get; set; }
    public string PlaylistTitle { get; set; } = string.Empty;
    public int UserId { get; set; }
}
