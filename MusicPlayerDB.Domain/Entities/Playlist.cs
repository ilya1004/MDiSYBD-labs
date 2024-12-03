using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class Playlist
{
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("is_public")]
    public bool IsPublic { get; set; } = false;

    [Column("user_id")]
    public int UserId { get; set; }

    [NotMapped]
    public User? User { get; set; } = null;
}
