using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class FavouriteSongs
{
    public int Id { get; set; }

    [Column("date_added")]
    public DateOnly DateAdded { get; set; } = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

    [Column("user_id")]
    public int UserId { get; set; }

    [NotMapped]
    public User? User { get; set; } = null;

    [Column("song_id")]
    public int SongId { get; set; }

    [NotMapped]
    public Song? Song { get; set; } = null;
}

