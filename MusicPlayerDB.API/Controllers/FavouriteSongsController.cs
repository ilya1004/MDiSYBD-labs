using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;

namespace MusicPlayerDB.API.Controllers;


[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class FavouriteSongsController : ControllerBase
{
    private readonly FavouriteSongsService _favouriteSongsService;

    public FavouriteSongsController(FavouriteSongsService favouriteSongsService)
    {
        _favouriteSongsService = favouriteSongsService;
    }

    /// <summary>
    /// Получить статистику добавлений песен в любимые.
    /// </summary>
    [HttpGet("stats/{artistId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetFavouriteSongStats(int artistId, [FromQuery] int playsCountThreshold = 15)
    {
        var result = await _favouriteSongsService.GetFavouriteSongStats(artistId, playsCountThreshold);
        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Получить общую длительность всех любимых песен для конкретного пользователя.
    /// </summary>
    [HttpGet("total-length/{userId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetTotalFavouritesLength(int userId)
    {
        if (userId <= 0)
            return BadRequest("Invalid user ID.");

        var result = await _favouriteSongsService.GetTotalFavouritesLength(userId);
        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Получить все любимые песни для конкретного пользователя.
    /// </summary>
    [HttpGet("by-user/{userId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetFavouriteSongs(int userId)
    {
        if (userId <= 0)
            return BadRequest("Invalid user ID.");

        var result = await _favouriteSongsService.GetFavouriteSongs(userId);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Добавить песню в любимые.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> AddFavouriteSong([FromBody] AddFavouriteSongRequest request)
    {
        if (request == null)
            return BadRequest("Request cannot be null.");

        var result = await _favouriteSongsService.AddFavouriteSong(request.UserId, request.SongId);
        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Удалить песню из любимых.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> DeleteFavouriteSong(int id)
    {
        var result = await _favouriteSongsService.Delete(id);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    ///// <summary>
    ///// Удалить песню из любимых.
    ///// </summary>
    //[HttpDelete("by-song/{songId}")]
    //public async Task<IActionResult> DeleteFavouriteSong(int songId)
    //{
    //    var result = await _favouriteSongsService.DeleteBySongId(songId);
     
    //    if (!result.Successfull)
    //        return BadRequest(result);

    //    return Ok(result);
    //}
}

/// <summary>
/// DTO для запроса добавления песни в любимые.
/// </summary>
public class AddFavouriteSongRequest
{
    public int UserId { get; set; }
    public int SongId { get; set; }
}
