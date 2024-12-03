using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class SongsRepository
{
    private readonly MusicPlayerDbContext _context;

    public SongsRepository(MusicPlayerDbContext context)
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
    /// Получить все песни.
    /// </summary>
    public Task<ResponseData<List<Song>>> GetAllSongsAsync()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Song>(
                FormattableStringFactory.Create("SELECT * FROM songs ORDER BY id")).AsNoTracking().ToListAsync();

            return data;
        });
    }

    /// <summary>
    /// Получить песню по ID.
    /// </summary>
    public Task<ResponseData<Song>> GetSongByIdAsync(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Song>(
                FormattableStringFactory.Create("SELECT * FROM songs WHERE id = {0}", id)).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Song with ID {id} not found");
        });
    }

    /// <summary>
    /// Добавить новую песню.
    /// </summary>
    public Task<ResponseData<bool>> AddSongAsync(Song song)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("INSERT INTO songs (title, release_year, artist_id) VALUES ({0}, {1}, {2})", song.Title!, song.ReleaseYear, song.ArtistId));

            return rows > 0;
        });
    }

    /// <summary>
    /// Обновить информацию о песне.
    /// </summary>
    public Task<ResponseData<bool>> UpdateSongAsync(int id, Song song)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("UPDATE songs SET title = {0}, release_year = {1}, artist_id = {2} WHERE id = {3}", song.Title!, song.ReleaseYear, song.ArtistId, id));

            return rows > 0;
        });
    }

    /// <summary>
    /// Удалить песню по ID.
    /// </summary>
    public Task<ResponseData<bool>> DeleteSongAsync(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("DELETE FROM songs WHERE id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Song with ID {id} not found");

            return rows > 0;
        });
    }

    /// <summary>
    /// Создание новой песни.
    /// </summary>
    public Task<ResponseData<int>> Create(Song song)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("INSERT INTO songs (title, duration_secs, release_year, artist_id, genre_id, album_id, plays_count) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                song.Title, song.DurationSecs, song.ReleaseYear, song.ArtistId, song.GenreId, song.AlbumId, song.PlaysCount));
            
            return rows;
        });
    }

    /// <summary>
    /// Получить все песни.
    /// </summary>
    public Task<ResponseData<List<Song>>> GetAll()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Song>(
                FormattableStringFactory.Create("SELECT * FROM songs ORDER BY title ASC")).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить песню по ID.
    /// </summary>
    public Task<ResponseData<Song>> GetById(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Song>(
                FormattableStringFactory.Create("SELECT * FROM songs WHERE id = {0}", id)).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Song with ID {id} not found");
        });
    }

    /// <summary>
    /// Обновление информации о песне.
    /// </summary>
    public Task<ResponseData<int>> Update(int id, Song song)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("UPDATE songs SET title = {0}, duration_secs = {1}, release_year = {2}, artist_id = {3}, genre_id = {4}, album_id = {5}, plays_count = {6} WHERE id = {7}",
                song.Title, song.DurationSecs, song.ReleaseYear, song.ArtistId, song.GenreId, song.AlbumId, song.PlaysCount, id));
            return rows;
        });
    }

    /// <summary>
    /// Удаление песни по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("DELETE FROM songs WHERE id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Song with ID {id} not found");

            return rows;
        });
    }

    /// <summary>
    /// Получить песни по альбому.
    /// </summary>
    public Task<ResponseData<List<Song>>> GetByAlbumId(int albumId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Song>(
                FormattableStringFactory.Create("SELECT * FROM songs WHERE album_id = {0}", albumId))
                .AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить песни по исполнителю.
    /// </summary>
    public Task<ResponseData<List<Song>>> GetByArtistId(int artistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Song>(
                FormattableStringFactory.Create("SELECT * FROM songs WHERE artist_id = {0}", artistId))
                .AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить песни по жанру.
    /// </summary>
    public Task<ResponseData<List<Song>>> GetByGenreId(int genreId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Song>(
                FormattableStringFactory.Create("SELECT * FROM songs WHERE genre_id = {0}", genreId))
                .AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Средняя оценка по песням.
    /// </summary>
    public Task<ResponseData<List<SongAverageRatingDTO>>> GetAverageSongRating(int limit = 20, int offset = 0)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            SELECT
                s.id AS Id,
                s.title AS Title, 
                ai.name AS ArtistName, 
                AVG(r.rating) AS AverageRating, 
                CASE 
                    WHEN AVG(r.rating) >= 4 THEN 'High rating'
                    WHEN AVG(r.rating) BETWEEN 3 AND 4 THEN 'Medium rating'
                    ELSE 'Low rating' 
                END AS RatingCategory
            FROM songs s
            INNER JOIN reviews r ON r.song_id = s.id
            INNER JOIN users u ON s.artist_id = u.id
            INNER JOIN artist_info ai ON u.artist_info_id = ai.id
            GROUP BY s.id, ai.name
            ORDER BY AverageRating DESC 
            LIMIT {0} OFFSET {1}", limit, offset);

            var data = await _context.Database.SqlQuery<SongAverageRatingDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }


    /// <summary>
    /// Количество добавлений в любимые.
    /// </summary>
    public Task<ResponseData<List<SongDetailDTO>>> GetFavoriteSongCount(int artistId, int playsCountThreshold = 15)
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
            ORDER BY s.plays_count DESC, FavouritesCount DESC", artistId, playsCountThreshold);

            var data = await _context.Database.SqlQuery<SongDetailDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }


    /// <summary>
    /// Песни по альбому.
    /// </summary>
    public Task<ResponseData<List<SongDetailDTO>>> GetSongsByAlbum(int albumId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            SELECT 
                s.id AS SongId,
                s.title AS Title,
                ai.name AS ArtistName,
                a.title AS AlbumName,
                s.duration_secs AS DurationSeconds,
                s.plays_count AS PlaysCount
            FROM songs s
            INNER JOIN albums a ON s.album_id = a.id
            INNER JOIN users u ON s.artist_id = u.id
            INNER JOIN artist_info ai ON u.artist_info_id = ai.id
            WHERE s.album_id = {0}
            ORDER BY s.id DESC", albumId);

            var data = await _context.Database.SqlQuery<SongDetailDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }


    /// <summary>
    /// Песни по плейлисту.
    /// </summary>
    public Task<ResponseData<List<SongByPlaylistDTO>>> GetSongsByPlaylist(int playlistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            SELECT 
                s.id AS Id,
                s.title AS Title,
                s.duration_secs AS DurationSecs,
                ai.name AS ArtistName,
                p.title AS PlaylistName
            FROM songs s
            INNER JOIN users u ON s.artist_id = u.id
            INNER JOIN artist_info ai ON u.artist_info_id = ai.id
            INNER JOIN playlists_songs ps ON ps.song_id = s.id
            INNER JOIN playlists p ON ps.playlist_id = p.id
            WHERE p.id = {0}
            ORDER BY s.id ASC", playlistId);

            var data = await _context.Database.SqlQuery<SongByPlaylistDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }

    /// <summary>
    /// Песни в плейлистах по ID пользователя.
    /// </summary>
    public Task<ResponseData<List<SongInPlaylistDTO>>> GetSongsInPlaylistByUser(int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                select s.id as SongId, s.title as SongTitle, ps.playlist_id as PlaylistId, 
                    p.title as PlaylistTitle, p.user_id as UserId
                from songs as s
                inner join playlists_songs as ps on ps.song_id = s.id
                inner join playlists as p on ps.playlist_id = p.id
                where p.user_id = {0}
                order by s.id", userId);

            var data = await _context.Database.SqlQuery<SongInPlaylistDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }



    /// <summary>
    /// Песни по жанру.
    /// </summary>
    public Task<ResponseData<List<SongDetailDTO>>> GetSongsByGenre(int genreId, int limit = 20, int offset = 0)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            SELECT 
                s.id AS SongId, 
                s.title AS Title, 
                u.id AS ArtistId, 
                ai.name AS ArtistName, 
                g.id AS GenreId, 
                g.name AS GenreName 
            FROM songs s
            INNER JOIN users u ON s.artist_id = u.id
            INNER JOIN artist_info ai ON u.artist_info_id = ai.id
            INNER JOIN genres g ON s.genre_id = g.id
            WHERE g.id = {0}
            ORDER BY s.id ASC
            LIMIT {1} OFFSET {2}", genreId, limit, offset);

            var data = await _context.Database.SqlQuery<SongDetailDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }


    /// <summary>
    /// Песни по тегу.
    /// </summary>
    public Task<ResponseData<List<SongDetailDTO>>> GetSongsByTag(int tagId, int limit = 20, int offset = 0)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            SELECT 
                s.id AS SongId, 
                s.title AS Title, 
                u.id AS ArtistId, 
                ai.name AS ArtistName, 
                t.id AS TagId, 
                t.name AS TagName 
            FROM songs s
            INNER JOIN users u ON s.artist_id = u.id
            INNER JOIN artist_info ai ON u.artist_info_id = ai.id
            INNER JOIN songs_tags st ON st.song_id = s.id
            INNER JOIN tags t ON st.tag_id = t.id
            WHERE t.id = {0}
            ORDER BY s.id ASC
            LIMIT {1} OFFSET {2}", tagId, limit, offset);

            var data = await _context.Database.SqlQuery<SongDetailDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }

    /// <summary>
    /// Обновление счетчика прослушиваний для песни.
    /// </summary>
    public Task<ResponseData<bool>> UpdateSongPlaysCount(int songId, int count)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            CALL update_song_plays_count({0}, {1})", songId, count);

            var result = await _context.Database.ExecuteSqlAsync(query);

            return result > 0;
        });
    }

    /// <summary>
    /// Добавление песни в список избранных пользователя.
    /// </summary>
    public Task<ResponseData<bool>> AddSongToFavourite(int songId, int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
            CALL add_song_to_favourite({0}, {1})", songId, userId);

            var result = await _context.Database.ExecuteSqlAsync(query);

            return result > 0;
        });
    }

}


