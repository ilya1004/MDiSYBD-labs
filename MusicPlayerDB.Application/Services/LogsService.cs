using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class LogsService
{
    private readonly LogsRepository _logsRepository;

    public LogsService(LogsRepository logsRepository)
    {
        _logsRepository = logsRepository;
    }

    /// <summary>
    /// Получить все логи.
    /// </summary>
    public async Task<ResponseData<List<Log>>> GetLogsAsync()
    {
        return await _logsRepository.GetLogsAsync();
    }

    /// <summary>
    /// Получить лог по ID.
    /// </summary>
    public async Task<ResponseData<Log>> GetLogByIdAsync(int logId)
    {
        return await _logsRepository.GetLogByIdAsync(logId);
    }

    /// <summary>
    /// Получение логов по дате.
    /// </summary>
    public async Task<ResponseData<List<Log>>> GetLogsByDateAsync(DateTime startDate, DateTime endDate)
    {
        return await _logsRepository.GetLogsByDateAsync(startDate, endDate);
    }
}
