using System.ComponentModel.DataAnnotations;

namespace MusicPlayerDB.Domain.DTOs;

public record UserCredentialsDTO
{
    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
