using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly ArtistsService _artistsService;

    public ArtistsController(ArtistsService artistsService)
    {
        _artistsService = artistsService;
    }

    /// <summary>
    /// Получить список артистов с пагинацией.
    /// </summary>
    [HttpGet("all")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetArtistsAsync(int limit = 10, int offset = 0)
    {
        var response = await _artistsService.GetArtistsAsync(limit, offset);

        if (!response.Successfull)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Получить информацию об артисте по ID.
    /// </summary>
    [HttpGet("{artistId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetArtistByIdAsync(int artistId)
    {
        var response = await _artistsService.GetArtistByIdAsync(artistId);

        if (!response.Successfull)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Получить артистов, отсортированных по количеству прослушиваний.
    /// </summary>
    [HttpGet("by-plays")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetArtistsByPlaysCount()
    {
        var response = await _artistsService.GetArtistsByPlaysCount();

        if (!response.Successfull)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

}

