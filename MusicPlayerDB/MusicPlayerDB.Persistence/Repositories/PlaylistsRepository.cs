using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class PlaylistsRepository
{
    private readonly MusicPlayerDbContext _context;

    public PlaylistsRepository(MusicPlayerDbContext context)
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
    /// Создание нового плейлиста.
    /// </summary>
    public Task<ResponseData<int>> Create(Playlist playlist)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    insert into playlists (title, description, is_public, user_id)
                    values ({0}, {1}, {2}, {3})", playlist.Title, playlist.Description, playlist.IsPublic, playlist.UserId)
            );
            return rows;
        });
    }

    /// <summary>
    /// Получение всех плейлистов.
    /// </summary>
    public Task<ResponseData<List<Playlist>>> GetAll()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Playlist>(
                FormattableStringFactory.Create(@"
                    select id, title, description, is_public, user_id
                    from playlists")
            ).ToListAsync();

            return data;
        });
    }

    /// <summary>
    /// Получение плейлиста по ID.
    /// </summary>
    public Task<ResponseData<Playlist>> GetById(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Playlist>(
                FormattableStringFactory.Create(@"
                    select * from playlists where id = {0}", id)
            ).FirstOrDefaultAsync();

            return data ?? throw new KeyNotFoundException($"Playlist with ID {id} not found");
        });
    }

    /// <summary>
    /// Обновление информации о плейлисте.
    /// </summary>
    public Task<ResponseData<int>> Update(int id, Playlist playlist)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    update playlists
                    set title = {0}, description = {1}, is_public = {2}
                    where id = {3}", playlist.Title, playlist.Description, playlist.IsPublic, id)
            );
            return rows;
        });
    }

    /// <summary>
    /// Удаление плейлиста по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    delete from playlists
                    where id = {0}", id)
            );

            if (rows == 0)
                throw new KeyNotFoundException($"Playlist with ID {id} not found");

            return rows;
        });
    }

    /// <summary>
    /// Получение длительности всех песен в плейлисте по его ID.
    /// </summary>
    public Task<ResponseData<int>> GetPlaylistLength(int playlistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var result = await _context.Database.SqlQuery<int>(
                FormattableStringFactory.Create(@"
                    select p.id, p.title, sum(s.duration_secs) playlist_length_secs
                    from playlists p
                    inner join playlists_songs ps on ps.playlist_id = p.id
                    inner join songs s on ps.song_id = s.id
                    inner join users u on s.artist_id = u.id
                    inner join artist_info ai on u.artist_info_id = ai.id
                    where p.id = {0}
                    group by p.id, p.title", playlistId)
            ).FirstOrDefaultAsync();

            return result;
        });
    }

    /// <summary>
    /// Получение всех плейлистов пользователя по его ID.
    /// </summary>
    public Task<ResponseData<List<Playlist>>> GetPlaylistsByUserId(int userId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Playlist>(
                FormattableStringFactory.Create(@"
                    select * from playlists
                    where user_id = {0}", userId)
            ).ToListAsync();

            return data;
        });
    }

    /// <summary>
    /// Изменение публичности плейлиста.
    /// </summary>
    public Task<ResponseData<int>> UpdatePlaylistVisibility(int playlistId, bool isPublic)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rowsAffected = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    update playlists
                    set is_public = {0}
                    where id = {1}", isPublic, playlistId)
            );

            return rowsAffected;
        });
    }

    /// <summary>
    /// Добавление песни в плейлист.
    /// </summary>
    public Task<ResponseData<int>> AddSongToPlaylist(int songId, int playlistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rowsAffected = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    insert into playlists_songs (song_id, playlist_id)
                    values ({0}, {1})", songId, playlistId)
            );

            return rowsAffected;
        });
    }

    /// <summary>
    /// Удаление песни из плейлиста.
    /// </summary>
    public Task<ResponseData<int>> DeleteSongFromPlaylist(int songId, int playlistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rowsAffected = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    delete from playlists_songs
                    where song_id = {0} and playlist_id = {1}", songId, playlistId)
            );

            return rowsAffected;
        });
    }

    /// <summary>
    /// Удаление всех песен из плейлиста.
    /// </summary>
    public Task<ResponseData<int>> DeleteAllSongsFromPlaylist(int playlistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rowsAffected = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create(@"
                    delete from playlists_songs
                    where playlist_id = {0}", playlistId)
            );

            return rowsAffected;
        });
    }
}

