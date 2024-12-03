using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class ReviewsRepository
{
    private readonly MusicPlayerDbContext _context;

    public ReviewsRepository(MusicPlayerDbContext context)
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
    /// Создание нового отзыва.
    /// </summary>
    public Task<ResponseData<int>> Create(Review review)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("INSERT INTO reviews (title, text, rating, date, user_id) VALUES ({0}, {1}, {2}, {3}, {4})",
                review.Title, review.Text, review.Rating, review.Date, review.UserId));
            return rows;
        });
    }

    /// <summary>
    /// Получить все отзывы.
    /// </summary>
    public Task<ResponseData<List<Review>>> GetAll()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Review>(
                FormattableStringFactory.Create("SELECT * FROM reviews ORDER BY date DESC")).AsNoTracking().ToListAsync();
            return data;
        });
    }

    /// <summary>
    /// Получить отзыв по ID.
    /// </summary>
    public Task<ResponseData<Review>> GetById(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Review>(
                FormattableStringFactory.Create("SELECT * FROM reviews WHERE id = {0}", id)).AsNoTracking().ToListAsync();

            return data.FirstOrDefault() ?? throw new KeyNotFoundException($"Review with ID {id} not found");
        });
    }

    /// <summary>
    /// Обновление информации об отзыве.
    /// </summary>
    public Task<ResponseData<int>> Update(int id, Review review)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("UPDATE reviews SET title = {0}, text = {1}, rating = {2}, date = {3} WHERE id = {4}",
                review.Title, review.Text, review.Rating, review.Date, id));
            return rows;
        });
    }

    /// <summary>
    /// Удаление отзыва по ID.
    /// </summary>
    public Task<ResponseData<int>> Delete(int id)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var rows = await _context.Database.ExecuteSqlAsync(
                FormattableStringFactory.Create("DELETE FROM reviews WHERE id = {0}", id));

            if (rows == 0)
                throw new KeyNotFoundException($"Review with ID {id} not found");

            return rows;
        });
    }

    /// <summary>
    /// Получить отзывы по песне.
    /// </summary>
    public Task<ResponseData<List<Review>>> GetBySongId(int songId, int limit = 10, int offset = 0)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<Review>(
                FormattableStringFactory.Create("SELECT * FROM reviews WHERE song_id = {0} LIMIT {1} OFFSET {2}", songId, limit, offset))
                .AsNoTracking().ToListAsync();
            return data;
        });
    }
}

