using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class ArtistsRepository
{
    private readonly MusicPlayerDbContext _context;

    public ArtistsRepository(MusicPlayerDbContext context)
    {
        _context = context;
    }

    private async Task<ResponseData<T>> ExecuteWithErrorHandling<T>(Func<Task<T>> func)
    {
        try
        {
            return ResponseData<T>.Success(await func());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
            return ResponseData<T>.Error(ex.Message);
        }
    }

    /// <summary>
    /// Получить список артистов с ограничением (10 артистов, начиная с определенного смещения).
    /// </summary>
    public Task<ResponseData<List<ArtistDetailDTO>>> GetArtistsAsync(int limit = 10, int offset = 0)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT u.id, u.login, u.password_hash, u.is_blocked, ai.name, ai.description, ai.country, ai.birthday
                FROM users u
                INNER JOIN artist_info ai ON u.artist_info_id = ai.id
                INNER JOIN roles r ON u.role_id = r.id
                WHERE r.name = 'Artist'
                ORDER BY u.id ASC
                LIMIT {0} OFFSET {1}", limit, offset);

            var data = await _context.Database.SqlQuery<ArtistDetailDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }

    /// <summary>
    /// Получить информацию об артисте по его ID.
    /// </summary>
    public Task<ResponseData<ArtistDetailDTO>> GetArtistByIdAsync(int artistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT u.id, u.login, u.password_hash, u.is_blocked, ai.name, ai.description, ai.country, ai.birthday
                FROM users u
                INNER JOIN artist_info ai ON u.artist_info_id = ai.id
                WHERE u.id = {0}", artistId);

            var data = await _context.Database.SqlQuery<ArtistDetailDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Artist with ID {artistId} not found");
        });
    }

    /// <summary>
    /// Артисты по количеству прослушиваний.
    /// </summary>
    public Task<ResponseData<List<ArtistPlaysCountDTO>>> GetArtistsByPlaysCount()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            SELECT 
                u.id AS Id, 
                ai.name AS ArtistName, 
                SUM(s.plays_count) AS PlaysCount 
            FROM songs s
            INNER JOIN users u ON s.artist_id = u.id
            INNER JOIN artist_info ai ON u.artist_info_id = ai.id
            GROUP BY u.id, ai.name
            ORDER BY PlaysCount DESC");

            var data = await _context.Database.SqlQuery<ArtistPlaysCountDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }
}
