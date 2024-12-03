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
public class EventsController : ControllerBase
{
    private readonly EventsService _eventsService;

    public EventsController(EventsService eventsService)
    {
        _eventsService = eventsService;
    }

    /// <summary>
    /// Создание нового события.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<IActionResult> Create([FromBody] Event eventData)
    {
        if (eventData == null)
            return BadRequest("Event cannot be null.");

        var result = await _eventsService.Create(eventData);

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Получить все события.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _eventsService.GetAll();

        if (result.Successfull)
            return Ok(result.Data);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Получить событие по ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid event ID.");

        var result = await _eventsService.GetById(id);

        if (result.Successfull)
            return Ok(result.Data);

        return NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Получить события для артиста по ID.
    /// </summary>
    [HttpGet("artist/{artistId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetByArtistId(int artistId)
    {
        if (artistId <= 0)
            return BadRequest("Invalid artist ID.");

        var result = await _eventsService.GetByArtistId(artistId);

        if (result.Successfull)
            return Ok(result.Data);

        return NotFound(result.ErrorMessage);
    }

    /// <summary>
    /// Обновить информацию о событии.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<IActionResult> Update(int id, [FromBody] Event eventData)
    {
        if (id <= 0)
            return BadRequest("Invalid event ID.");

        if (eventData == null)
            return BadRequest("Event cannot be null.");

        var result = await _eventsService.Update(id, eventData);

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Удалить событие по ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid event ID.");

        var result = await _eventsService.Delete(id);

        if (result.Successfull)
            return Ok(result);

        return NotFound(result.ErrorMessage);
    }
}
