using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class PlaylistsService
{
    private readonly PlaylistsRepository _playlistsRepository;

    public PlaylistsService(PlaylistsRepository playlistsRepository)
    {
        _playlistsRepository = playlistsRepository;
    }

    /// <summary>
    /// Создание нового плейлиста.
    /// </summary>
    public async Task<ResponseData<int>> Create(Playlist playlist)
    {
        if (playlist == null)
            return ResponseData<int>.Error("Playlist cannot be null.");

        return await _playlistsRepository.Create(playlist);
    }

    /// <summary>
    /// Получить все плейлисты.
    /// </summary>
    public async Task<ResponseData<List<Playlist>>> GetAll()
    {
        return await _playlistsRepository.GetAll();
    }

    /// <summary>
    /// Получить плейлист по ID.
    /// </summary>
    public async Task<ResponseData<Playlist>> GetById(int id)
    {
        if (id <= 0)
            return ResponseData<Playlist>.Error("Invalid playlist ID.");

        return await _playlistsRepository.GetById(id);
    }

    /// <summary>
    /// Обновление информации о плейлисте.
    /// </summary>
    public async Task<ResponseData<int>> Update(int id, Playlist playlist)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid playlist ID.");

        if (playlist == null)
            return ResponseData<int>.Error("Playlist cannot be null.");

        return await _playlistsRepository.Update(id, playlist);
    }

    /// <summary>
    /// Удаление плейлиста по ID.
    /// </summary>
    public async Task<ResponseData<int>> Delete(int id)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid playlist ID.");

        return await _playlistsRepository.Delete(id);
    }

    /// <summary>
    /// Получить длительность всех песен в плейлисте.
    /// </summary>
    public async Task<ResponseData<int>> GetPlaylistLength(int playlistId)
    {
        if (playlistId <= 0)
            return ResponseData<int>.Error("Invalid playlist ID.");

        return await _playlistsRepository.GetPlaylistLength(playlistId);
    }

    /// <summary>
    /// Получить все плейлисты пользователя.
    /// </summary>
    public async Task<ResponseData<List<Playlist>>> GetPlaylistsByUserId(int userId)
    {
        if (userId <= 0)
            return ResponseData<List<Playlist>>.Error("Invalid user ID.");

        return await _playlistsRepository.GetPlaylistsByUserId(userId);
    }

    /// <summary>
    /// Изменить публичность плейлиста.
    /// </summary>
    public async Task<ResponseData<int>> UpdatePlaylistVisibility(int playlistId, bool isPublic)
    {
        if (playlistId <= 0)
            return ResponseData<int>.Error("Invalid playlist ID.");

        return await _playlistsRepository.UpdatePlaylistVisibility(playlistId, isPublic);
    }

    /// <summary>
    /// Добавить песню в плейлист.
    /// </summary>
    public async Task<ResponseData<int>> AddSongToPlaylist(int songId, int playlistId)
    {
        if (songId <= 0 || playlistId <= 0)
            return ResponseData<int>.Error("Invalid song ID or playlist ID.");

        return await _playlistsRepository.AddSongToPlaylist(songId, playlistId);
    }

    /// <summary>
    /// Удалить песню из плейлиста.
    /// </summary>
    public async Task<ResponseData<int>> DeleteSongFromPlaylist(int songId, int playlistId)
    {
        if (songId <= 0 || playlistId <= 0)
            return ResponseData<int>.Error("Invalid song ID or playlist ID.");

        return await _playlistsRepository.DeleteSongFromPlaylist(songId, playlistId);
    }

    /// <summary>
    /// Удалить песни из плейлиста.
    /// </summary>
    public async Task<ResponseData<int>> DeleteAllSongsFromPlaylist(int playlistId)
    {
        if (playlistId <= 0)
            return ResponseData<int>.Error("Invalid playlist ID.");

        return await _playlistsRepository.DeleteAllSongsFromPlaylist(playlistId);
    }
}

