using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.DTOs;

public record ArtistDetailDTO
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("is_blocked")]
    public bool IsBlocked { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}
