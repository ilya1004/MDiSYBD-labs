namespace MusicPlayerDB.Application.Interfaces;

public interface IPasswordHasher
{
    string GenerateHash(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
