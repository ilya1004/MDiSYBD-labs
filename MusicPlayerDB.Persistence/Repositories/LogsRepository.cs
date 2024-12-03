using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class LogsRepository
{
    private readonly MusicPlayerDbContext _context;

    public LogsRepository(MusicPlayerDbContext context)
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
    /// Получение всех логов.
    /// </summary>
    public Task<ResponseData<List<Log>>> GetLogsAsync()
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT l.id, l.action, l.datetime, l.user_id
                FROM logs l
                LEFT JOIN users u ON l.user_id = u.id");

            var logs = await _context.Database.SqlQuery<Log>(query)
                .AsNoTracking()
                .ToListAsync();

            return logs;
        });
    }

    /// <summary>
    /// Получение лога по ID.
    /// </summary>
    public Task<ResponseData<Log>> GetLogByIdAsync(int logId)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT l.id, l.action, l.datetime, l.user_id
                FROM logs l
                LEFT JOIN users u ON l.user_id = u.id
                WHERE l.id = {0}", logId);

            var log = await _context.Database.SqlQuery<Log>(query)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return log ?? throw new KeyNotFoundException($"Log with ID {logId} not found");
        });
    }

    /// <summary>
    /// Получение логов по дате.
    /// </summary>
    public Task<ResponseData<List<Log>>> GetLogsByDateAsync(DateTime startDate, DateTime endDate)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var query = FormattableStringFactory.Create(@"
                SELECT l.id, l.action, l.datetime, l.user_id
                FROM user_logs l
                WHERE '{0}' <= l.datetime AND l.datetime < '{1}'", startDate, endDate);

            var logs = await _context.Database.SqlQuery<Log>(query)
                .AsNoTracking()
                .ToListAsync();

            return logs;
        });
    }
}


