using MusicPlayerDB.Domain.Entities;

namespace MusicPlayerDB.Application.Interfaces;

public interface IJwtProvider
{
    public string GenerateToken(int id, int roleId);
}