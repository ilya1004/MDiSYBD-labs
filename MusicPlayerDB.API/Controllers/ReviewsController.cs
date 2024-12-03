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
public class ReviewsController : ControllerBase
{
    private readonly ReviewsService _reviewsService;

    public ReviewsController(ReviewsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    /// <summary>
    /// Создание нового отзыва.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> Create([FromBody] Review review)
    {
        var result = await _reviewsService.Create(review);

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Получить все отзывы.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _reviewsService.GetAll();

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Получить отзыв по ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _reviewsService.GetById(id);

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Обновление информации об отзыве.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> Update(int id, [FromBody] Review review)
    {
        var result = await _reviewsService.Update(id, review);

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Удаление отзыва по ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reviewsService.Delete(id);

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    /// <summary>
    /// Получить отзывы по ID песни с пагинацией.
    /// </summary>
    [HttpGet("song/{songId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetBySongId(int songId, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        var result = await _reviewsService.GetBySongId(songId, limit, offset);

        if (result.Successfull)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }
}

