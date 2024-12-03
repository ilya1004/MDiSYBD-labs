using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("is_blocked")]
    public bool IsBlocked { get; set; } = false;

    [Column("role_id")]
    public int RoleId { get; set; }

    [NotMapped]
    public Role? Role { get; set; } = null;

    [Column("user_info_id")]
    public int? UserInfoId { get; set; }

    [NotMapped]
    public UserInfo? UserInfo { get; set; } = null;

    [Column("artist_info_id")]
    public int? ArtistInfoId { get; set; }

    [NotMapped]
    public ArtistInfo? ArtistInfo { get; set; } = null;
}
