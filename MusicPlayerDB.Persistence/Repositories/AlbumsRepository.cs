using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class AlbumsRepository
{
    private readonly MusicPlayerDbContext _context;

    public AlbumsRepository(MusicPlayerDbContext context)
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
    /// Создание нового альбома.
    /// </summary>
    public Task<ResponseData<int>> Create(Album album)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("INSERT INTO albums (title, releaseYear, artistId) VALUES ({0}, {1}, {2})",
                album.Title, album.ReleaseYear, album.ArtistId));
            return rows;
        });
    }

    /// <summary>
    /// Получить все альбомы.
    /// </summary>
    public Task<ResponseData<List<Album>>> GetAll()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Album>(
                FormattableStringFactory.Create("SELECT * FROM albums ORDER BY id")).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить все альбомы по ID артиста.
    /// </summary>
    public Task<ResponseData<List<Album>>> GetByArtistId(int artistId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Album>(
                FormattableStringFactory.Create("SELECT * FROM albums WHERE artist_id = {0} ORDER BY id", artistId)).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить альбом по ID.
    /// </summary>
    public Task<ResponseData<Album>> GetById(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Album>(
                FormattableStringFactory.Create("SELECT * FROM albums WHERE id = {0}", id)).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Album with ID {id} not found");
        });
    }

    /// <summary>
    /// Обновление информации об альбоме.
    /// </summary>
    public Task<ResponseData<int>> Update(int id, Album album)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("UPDATE albums SET title = {0}, releaseYear = {1}, artistId = {2} WHERE id = {3}",
                album.Title, album.ReleaseYear, album.ArtistId, id));
            return rows;
        });
    }

    /// <summary>
    /// Удаление альбома по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("DELETE FROM albums WHERE id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Album with ID {id} not found");

            return rows;
        });
    }
}

