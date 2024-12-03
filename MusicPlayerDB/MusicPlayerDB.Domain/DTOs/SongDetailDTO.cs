using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.DTOs;


public class SongDetailDTO
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public int? DurationSecs { get; set; } = null;
    public int? PlaysCount { get; set; } = null;
    public string? ArtistName { get; set; } = string.Empty;
    public string? GenreName { get; set; } = null; 
    public string? PlaylistName { get; set; } = null;
    public string? AverageRating { get; set; } = null;
    public string? RatingCategory { get; set; } = null;
    public int? FavouritiesCount { get; set; } = null;
    public List<string>? Tags { get; set; } = [];
    public string? AlbumName { get; set; } = null;
    public bool? HasReviews { get; set; } = true;
    public int? SongRankByArtist { get; set; } = null;
}

