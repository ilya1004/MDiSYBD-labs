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
public class AlbumsController : ControllerBase
{
    private readonly AlbumsService _albumsService;

    public AlbumsController(AlbumsService albumsService)
    {
        _albumsService = albumsService;
    }

    /// <summary>
    /// Создание нового альбома.
    /// </summary>
    /// <param name="album">Информация о создаваемом альбоме</param>
    /// <returns>Результат выполнения операции</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<IActionResult> Create([FromBody] Album album)
    {
        if (album == null)
        {
            return BadRequest("Album data is required.");
        }

        var response = await _albumsService.Create(album);

        if (response.Successfull)
        {
            return CreatedAtAction(nameof(GetById), new { id = response.Data }, response);
        }

        return BadRequest(response);
    }

    /// <summary>
    /// Получить все альбомы.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _albumsService.GetAll();

        if (response.Successfull)
        {
            return Ok(response);
        }

        return NotFound(response);
    }

    /// <summary>
    /// Получить все альбомы.
    /// </summary>
    [HttpGet("by-artist/{artistId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetByArtist(int artistId)
    {
        var response = await _albumsService.GetByArtistId(artistId);

        if (response.Successfull)
        {
            return Ok(response);
        }

        return NotFound(response.ErrorMessage);
    }

    /// <summary>
    /// Получить альбом по ID.
    /// </summary>
    /// <param name="id">ID альбома</param>
    /// <returns>Информация о найденном альбоме</returns>
    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid album ID.");
        }

        var response = await _albumsService.GetById(id);

        if (response.Successfull)
        {
            return Ok(response);
        }

        return NotFound(response);
    }

    /// <summary>
    /// Обновление информации об альбоме.
    /// </summary>
    /// <param name="id">ID альбома</param>
    /// <param name="album">Новая информация об альбоме</param>
    /// <returns>Результат выполнения операции</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<IActionResult> Update(int id, [FromBody] Album album)
    {
        if (id <= 0 || album == null)
        {
            return BadRequest("Invalid data.");
        }

        var response = await _albumsService.Update(id, album);

        if (response.Successfull)
        {
            return NoContent();
        }

        return BadRequest(response);
    }

    /// <summary>
    /// Удаление альбома по ID.
    /// </summary>
    /// <param name="id">ID альбома</param>
    /// <returns>Результат выполнения операции</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid album ID.");
        }

        var response = await _albumsService.Delete(id);

        if (response.Successfull)
        {
            return NoContent();
        }

        return NotFound(response);
    }
}

