using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Column("datetime")]
    public DateTime DateTime { get; set; }
    public string Location { get; set; } = string.Empty;

    [Column("artist_id")]
    public int ArtistId { get; set; }

    [NotMapped]
    public User? Artist { get; set; } = null;
}
