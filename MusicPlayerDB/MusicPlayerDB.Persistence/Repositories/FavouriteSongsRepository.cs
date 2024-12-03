using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;
using MusicPlayerDB.Domain.DTOs;

namespace MusicPlayerDB.Persistence.Repositories;

public class FavouriteSongsRepository
{
    private readonly MusicPlayerDbContext _context;

    public FavouriteSongsRepository(MusicPlayerDbContext context)
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
    /// Выборка количества добавлений в любимые песни.
    /// </summary>
    public Task<ResponseData<List<FavouriteSongStats>>> GetFavouriteSongStats(int artistId, int playsCountThreshold)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT 
                    s.id AS SongId, 
                    s.title AS Title, 
                    s.duration_secs AS DurationSeconds, 
                    s.plays_count AS PlaysCount, 
                    COUNT(*) AS FavouritesCount
                FROM favourite_songs fss
                INNER JOIN songs s ON fss.song_id = s.id
                INNER JOIN users u ON s.artist_id = u.id
                WHERE s.artist_id = {0}
                GROUP BY s.id, fss.song_id
                HAVING s.plays_count > {1}
                ORDER BY s.plays_count DESC, FavouritesCount DESC",
                artistId,
                playsCountThreshold);

            var data = await _context.Database.SqlQuery<FavouriteSongStats>(query).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить общую длительность всех любимых песен для конкретного пользователя.
    /// </summary>
    public Task<ResponseData<int>> GetTotalFavouritesLength(int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(
                "SELECT SUM(s.duration_secs) AS favourities_length_secs FROM favourite_songs fss " +
                "INNER JOIN songs s ON fss.song_id = s.id WHERE fss.user_id = {0}", userId);

            var result = await _context.Database.SqlQuery<int>(query).ToListAsync();

            return result.FirstOrDefault();
        });
    }

    /// <summary>
    /// Получить все любимые песни для конкретного пользователя.
    /// </summary>
    public Task<ResponseData<List<FavouriteSongDTO>>> GetFavouriteSongs(int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(
                @"select fs.id as id, fs.date_added as dateAdded, s.id as songId, s.title as title, s.duration_secs as durationSecs, ai.name as artistName
                  from favourite_songs AS fs
                  inner join songs AS s on fs.song_id = s.id
                  inner join users as u on s.artist_id = u.id
                  inner join artist_info as ai on u.artist_info_id = ai.id
                  where fs.user_id = {0}
                  order by fs.date_added desc", userId);

            var result = await _context.Database.SqlQuery<FavouriteSongDTO>(query).ToListAsync();

            return result;
        });
    }

    /// <summary>
    /// Удаление любимой песни по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"delete from favourite_songs where id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Favourite Song with ID {id} not found");

            return rows;
        });
    }

    /// <summary>
    /// Добавление песни в любимые.
    /// </summary>
    public Task<ResponseData<int>> AddFavouriteSong(int userId, int songId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    INSERT INTO favourite_songs (user_id, song_id)
                    VALUES ({0}, {1})",
                    userId,
                    songId));
            return rows;
        });
    }
}
