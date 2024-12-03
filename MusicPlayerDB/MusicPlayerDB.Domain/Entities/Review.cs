using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateOnly Date { get; set; } = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

    [Column("user_id")]
    public int UserId { get; set; }

    [NotMapped]
    public User? User { get; set; } = null;

    [Column("song_id")]
    public int SongId { get; set; }

    [NotMapped]
    public Song? Song { get; set; } = null;
}
