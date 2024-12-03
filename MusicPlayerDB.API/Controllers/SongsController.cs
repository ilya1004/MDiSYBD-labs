using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly SongsService _songService;

    public SongsController(SongsService songService)
    {
        _songService = songService;
    }

    // Получение всех песен
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<Song>>>> GetAllSongs()
    {
        var result = await _songService.GetAllSongsAsync();
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    // Получение песни по ID
    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<Song>>> GetSongById(int id)
    {
        var result = await _songService.GetSongByIdAsync(id);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return NotFound(result);
    }

    // Добавление новой песни
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<int>>> AddSong([FromBody] Song song)
    {
        var result = await _songService.AddSongAsync(song);
        if (result.Successfull)
        {
            return CreatedAtAction(nameof(GetSongById), new { id = result.Data }, result);
        }
        return BadRequest(result);
    }

    // Обновление песни
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<bool>>> UpdateSong(int id, [FromBody] Song updatedSong)
    {
        var result = await _songService.UpdateSongAsync(id, updatedSong);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    // Удаление песни
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<bool>>> DeleteSong(int id)
    {
        var result = await _songService.DeleteSongAsync(id);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Получение списка песен с их средними оценками.
    /// </summary>
    [HttpGet("average-rating")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<SongAverageRatingDTO>>>> GetAverageSongRating([FromQuery] int limit = 20, [FromQuery] int offset = 0)
    {
        var result = await _songService.GetAverageSongRating(limit, offset);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Получение списка песен по идентификатору исполнителя.
    /// </summary>
    [HttpGet("by-artist/{artistId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<Song>>>> GetSongsByArtist(int artistId)
    {
        var result = await _songService.GetSongsByArtist(artistId);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Получение списка песен по идентификатору альбома.
    /// </summary>
    [HttpGet("by-album/{albumId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<Song>>>> GetSongsByAlbum(int albumId)
    {
        var result = await _songService.GetByAlbumId(albumId);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Получение списка песен по идентификатору плейлиста.
    /// </summary>
    [HttpGet("by-playlist/{playlistId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<SongByPlaylistDTO>>>> GetSongsByPlaylist(int playlistId)
    {
        var result = await _songService.GetSongsByPlaylist(playlistId);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("songs-in-playlists-by-user/{userId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<SongInPlaylistDTO>>>> GetSongsInPlaylistByUser(int userId)
    {
        var result = await _songService.GetSongsInPlaylistByUser(userId);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Получение списка песен по жанру.
    /// </summary>
    [HttpGet("by-genre/{genreId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<SongDetailDTO>>>> GetSongsByGenre(int genreId)
    {
        var result = await _songService.GetSongsByGenre(genreId);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Получение списка песен по тегу.
    /// </summary>
    [HttpGet("by-tag/{tagId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<List<SongDetailDTO>>>> GetSongsByTag(int tagId)
    {
        var result = await _songService.GetSongsByTag(tagId);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Обновление количества воспроизведений песни.
    /// </summary>
    [HttpPut("update-plays-count/{songId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<bool>>> UpdateSongPlaysCount(int songId, [FromBody] int count)
    {
        var result = await _songService.UpdateSongPlaysCount(songId, count);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// Добавление песни в избранное.
    /// </summary>
    [HttpPost("add-to-favourite")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<ActionResult<ResponseData<bool>>> AddSongToFavourite([FromQuery] int songId, [FromQuery] int userId)
    {
        var result = await _songService.AddSongToFavourite(songId, userId);
        if (result.Successfull)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}

