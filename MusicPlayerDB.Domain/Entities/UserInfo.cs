namespace MusicPlayerDB.Domain.Entities;

public class UserInfo
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
}
