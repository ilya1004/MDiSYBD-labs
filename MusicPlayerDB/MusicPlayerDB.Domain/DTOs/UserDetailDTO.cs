using MusicPlayerDB.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Domain.DTOs;


public class UserDetailDTO
{
    [Column("id")]
    public int Id { get; set; }

    [Column("login")]
    public string Login { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("is_blocked")]
    public bool IsBlocked { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("user_info_id")]
    public int? UserInfoId { get; set; }

    [Column("artist_info_id")]
    public int? ArtistInfoId { get; set; }

    [Column("nickname")]
    public string Nickname { get; set; } = string.Empty;

    [Column("about")]
    public string? About { get; set; } = string.Empty;
}

