using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class TagsRepository
{
    private readonly MusicPlayerDbContext _context;

    public TagsRepository(MusicPlayerDbContext context)
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
    /// Создание нового тега.
    /// </summary>
    public Task<ResponseData<int>> Create(Tag tag)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("INSERT INTO tags (name) VALUES ({0})", tag.Name!));
            return rows;
        });
    }

    /// <summary>
    /// Получить все теги.
    /// </summary>
    public Task<ResponseData<List<Tag>>> GetAll()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Tag>(
                FormattableStringFactory.Create("SELECT * FROM tags ORDER BY id")).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить тег по ID.
    /// </summary>
    public Task<ResponseData<Tag>> GetById(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Tag>(
                FormattableStringFactory.Create("SELECT * FROM tags WHERE id = {0}", id)).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Tag with ID {id} not found");
        });
    }

    /// <summary>
    /// Обновление информации о теге.
    /// </summary>
    public Task<ResponseData<int>> Update(int id, Tag tag)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("UPDATE tags SET name = {0} WHERE id = {1}", tag.Name!, id));
            return rows;
        });
    }

    /// <summary>
    /// Удаление тега по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("DELETE FROM tags WHERE id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Tag with ID {id} not found");

            return rows;
        });
    }

    /// <summary>
    /// Теги по ID песни.
    /// </summary>
    public Task<ResponseData<List<SongWithTagsDTO>>> GetTagsForSong(int songId)
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
            LEFT OUTER JOIN songs_tags st ON st.song_id = s.id
            LEFT OUTER JOIN tags t ON st.tag_id = t.id
            WHERE s.id = {0}
            ORDER BY s.id ASC", songId);

            var data = await _context.Database.SqlQuery<SongWithTagsDTO>(query)
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }
}
