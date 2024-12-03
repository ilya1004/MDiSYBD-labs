namespace MusicPlayerDB.Domain.DTOs;

public record SongAverageRatingDTO
{
    public int Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public decimal AverageRating { get; set; }
    public string RatingCategory { get; set; } = string.Empty;
}
