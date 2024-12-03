using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;
using MusicPlayerDB.Domain.Entities;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly GenresService _genresService;

    public GenresController(GenresService genresService)
    {
        _genresService = genresService;
    }

    /// <summary>
    /// Создание нового жанра.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> Create([FromBody] Genre genre)
    {
        if (genre == null)
            return BadRequest("Genre cannot be null.");

        var result = await _genresService.Create(genre);
        if (!result.Successfull)
            return BadRequest(result.ErrorMessage);

        return Ok(result);
    }

    /// <summary>
    /// Получить все жанры.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _genresService.GetAll();
        if (!result.Successfull)
            return BadRequest(result.ErrorMessage);

        return Ok(result);
    }

    /// <summary>
    /// Получить жанр по ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid genre ID.");

        var result = await _genresService.GetById(id);
        if (!result.Successfull)
            return NotFound(result.ErrorMessage);

        return Ok(result);
    }

    /// <summary>
    /// Обновление информации о жанре.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> Update(int id, [FromBody] Genre genre)
    {
        if (id <= 0)
            return BadRequest("Invalid genre ID.");

        if (genre == null)
            return BadRequest("Genre cannot be null.");

        var result = await _genresService.Update(id, genre);
        if (!result.Successfull)
            return BadRequest(result.ErrorMessage);

        return Ok(result);
    }

    /// <summary>
    /// Удаление жанра по ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid genre ID.");

        var result = await _genresService.Delete(id);
        if (!result.Successfull)
            return BadRequest(result.ErrorMessage);

        return Ok(result);
    }
}
