using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class EventsRepository
{
    private readonly MusicPlayerDbContext _context;

    public EventsRepository(MusicPlayerDbContext context)
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
    /// Создание нового события.
    /// </summary>
    public Task<ResponseData<int>> Create(Event eventItem)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("INSERT INTO events (title, description, datetime, location, artistId) VALUES ({0}, {1}, {2}, {3}, {4})",
                eventItem.Title, eventItem.Description, eventItem.DateTime, eventItem.Location, eventItem.ArtistId));
            return rows;
        });
    }

    /// <summary>
    /// Получить все события.
    /// </summary>
    public Task<ResponseData<List<Event>>> GetAll()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Event>(
                FormattableStringFactory.Create("SELECT * FROM events ORDER BY datetime")).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить событие по ID.
    /// </summary>
    public Task<ResponseData<Event>> GetById(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Event>(
                FormattableStringFactory.Create("SELECT * FROM events WHERE id = {0}", id)).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Event with ID {id} not found");
        });
    }

    /// <summary>
    /// Обновление информации о событии.
    /// </summary>
    public Task<ResponseData<int>> Update(int id, Event eventItem)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("UPDATE events SET title = {0}, description = {1}, datetime = {2}, location = {3}, artistId = {4} WHERE id = {5}",
                eventItem.Title, eventItem.Description, eventItem.DateTime, eventItem.Location, eventItem.ArtistId, id));
            return rows;
        });
    }

    /// <summary>
    /// Удаление события по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("DELETE FROM events WHERE id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Event with ID {id} not found");

            return rows;
        });
    }

    /// <summary>
    /// Получение событий для артиста по его имени.
    /// </summary>
    public async Task<ResponseData<List<Event>>> GetByArtistId(int artistId)
    {
        return await ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Event>(
                FormattableStringFactory.Create(@"SELECT * 
                  FROM events 
                  WHERE artist_id = 
                        (SELECT u.id 
                         FROM users u 
                         INNER JOIN artist_info ai ON u.artist_info_id = ai.id 
                         WHERE ai.name = (SELECT name FROM artist_info WHERE id = {0} LIMIT 1))
                  AND datetime > CURRENT_DATE
                  ORDER BY datetime ASC", artistId))
                .AsNoTracking()
                .ToListAsync();

            return data;
        });
    }
}

