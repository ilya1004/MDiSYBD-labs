using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class GenresRepository
{
    private readonly MusicPlayerDbContext _context;

    public GenresRepository(MusicPlayerDbContext context)
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
    /// Создание нового жанра.
    /// </summary>
    public Task<ResponseData<int>> Create(Genre genre)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("INSERT INTO genres (name, description) VALUES ({0}, {1})", genre.Name, genre.Description));
            return rows;
        });
    }

    /// <summary>
    /// Получить все жанры.
    /// </summary>
    public Task<ResponseData<List<Genre>>> GetAll()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Genre>(
                FormattableStringFactory.Create("SELECT * FROM genres ORDER BY id")).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить жанр по ID.
    /// </summary>
    public Task<ResponseData<Genre>> GetById(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Genre>(
                FormattableStringFactory.Create("SELECT * FROM genres WHERE id = {0}", id)).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Genre with ID {id} not found");
        });
    }

    /// <summary>
    /// Обновление информации о жанре.
    /// </summary>
    public Task<ResponseData<int>> Update(int id, Genre genre)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("UPDATE genres SET name = {0}, description = {1} WHERE id = {2}", genre.Name, genre.Description, id));
            return rows;
        });
    }

    /// <summary>
    /// Удаление жанра по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("DELETE FROM genres WHERE id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Genre with ID {id} not found");

            return rows;
        });
    }
}
