using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlayerDB.Domain.Entities;

public class Log
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;

    [Column("user_id")]
    public int UserId { get; set; }

    [NotMapped]
    public User? User { get; set; } = null;
}
