namespace MusicPlayerDB.Domain.DTOs;

public class SongWithTagsDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public int TagId { get; set; }
    public string TagName { get; set; } = string.Empty;
}
