using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly PlaylistsService _playlistsService;

    public PlaylistsController(PlaylistsService playlistsService)
    {
        _playlistsService = playlistsService;
    }

    /// <summary>
    /// Создание нового плейлиста.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.ArtistAndAdminPolicy)]
    public async Task<IActionResult> Create([FromBody] Playlist playlist)
    {
        var result = await _playlistsService.Create(playlist);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Получить все плейлисты.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _playlistsService.GetAll();

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Получить плейлист по ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _playlistsService.GetById(id);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Обновление информации о плейлисте.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> Update(int id, [FromBody] Playlist playlist)
    {
        var result = await _playlistsService.Update(id, playlist);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Удаление плейлиста по ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _playlistsService.Delete(id);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Получить длительность всех песен в плейлисте.
    /// </summary>
    [HttpGet("{playlistId}/length")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetPlaylistLength(int playlistId)
    {
        var result = await _playlistsService.GetPlaylistLength(playlistId);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Получить все плейлисты пользователя.
    /// </summary>
    [HttpGet("by-user/{userId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetPlaylistsByUserId(int userId)
    {
        var result = await _playlistsService.GetPlaylistsByUserId(userId);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Изменить публичность плейлиста.
    /// </summary>
    [HttpPut("{id}/visibility")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> UpdatePlaylistVisibility(int id, [FromQuery] bool isPublic)
    {
        var result = await _playlistsService.UpdatePlaylistVisibility(id, isPublic);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Добавить песню в плейлист.
    /// </summary>
    [HttpPost("{playlistId}/songs/{songId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> AddSongToPlaylist(int playlistId, int songId)
    {
        var result = await _playlistsService.AddSongToPlaylist(songId, playlistId);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Удалить песню из плейлиста.
    /// </summary>
    [HttpDelete("{playlistId}/songs/{songId}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> DeleteSongFromPlaylist(int playlistId, int songId)
    {
        var result = await _playlistsService.DeleteSongFromPlaylist(songId, playlistId);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Удалить песню из плейлиста.
    /// </summary>
    [HttpDelete("{playlistId}/all-songs")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> DeleteAllSongsFromPlaylist(int playlistId)
    {
        var result = await _playlistsService.DeleteAllSongsFromPlaylist(playlistId);

        if (!result.Successfull)
            return BadRequest(result);

        return Ok(result);
    }
}
