using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    [Column("duration_secs")]
    public int DurationSecs { get; set; }

    [Column("release_year")]
    public int ReleaseYear { get; set; }
    
    [Column("artist_id")]
    public int ArtistId { get; set; }

    [NotMapped]
    public User? Artist { get; set; } = null;

    [Column("genre_id")]
    public int? GenreId { get; set; }

    [NotMapped]
    public Genre? Genre { get; set; } = null;

    [Column("album_id")]
    public int? AlbumId { get; set; }

    [NotMapped]
    public Album? Album { get; set; } = null;

    [Column("plays_count")]
    public int PlaysCount { get; set; } = 0;
}
