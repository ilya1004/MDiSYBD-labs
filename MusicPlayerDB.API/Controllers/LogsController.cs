using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;
using MusicPlayerDB.Domain.Entities;

namespace MusicPlayerDB.API.Controllers;


[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly LogsService _logsService;

    public LogsController(LogsService logsService)
    {
        _logsService = logsService;
    }

    /// <summary>
    /// Получить все логи.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> GetLogs()
    {
        var response = await _logsService.GetLogsAsync();
        if (response.Successfull)
        {
            return Ok(response.Data);
        }
        return BadRequest(response.ErrorMessage);
    }

    /// <summary>
    /// Получить лог по ID.
    /// </summary>
    [HttpGet("{logId}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> GetLogById(int logId)
    {
        var response = await _logsService.GetLogByIdAsync(logId);
        if (response.Successfull)
        {
            return Ok(response.Data);
        }
        return NotFound(response.ErrorMessage);
    }

    /// <summary>
    /// Получение логов по дате.
    /// </summary>
    [HttpGet("by-date")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> GetLogsByDateAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = await _logsService.GetLogsByDateAsync(startDate, endDate);
        if (response.Successfull)
        {
            return Ok(response.Data);
        }
        return BadRequest(response.ErrorMessage);
    }
}

