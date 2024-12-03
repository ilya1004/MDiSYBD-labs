using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class Album
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    [Column("release_year")]
    public int ReleaseYear { get; set; }

    [Column("artist_id")]
    public int ArtistId { get; set; }

    [NotMapped]
    public User? Artist { get; set; } = null; 
}
