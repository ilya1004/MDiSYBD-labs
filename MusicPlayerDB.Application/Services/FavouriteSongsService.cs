using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class FavouriteSongsService
{
    private readonly FavouriteSongsRepository _favouriteSongsRepository;

    public FavouriteSongsService(FavouriteSongsRepository favouriteSongsRepository)
    {
        _favouriteSongsRepository = favouriteSongsRepository;
    }

    /// <summary>
    /// Получить статистику добавлений песен в любимые.
    /// </summary>
    public async Task<ResponseData<List<FavouriteSongStats>>> GetFavouriteSongStats(int artistId, int playsCountThreshold = 15)
    {
        if (artistId <= 0)
            return ResponseData<List<FavouriteSongStats>>.Error("Invalid artist ID.");

        return await _favouriteSongsRepository.GetFavouriteSongStats(artistId, playsCountThreshold);
    }

    /// <summary>
    /// Получить общую длительность всех любимых песен для конкретного пользователя.
    /// </summary>
    public async Task<ResponseData<int>> GetTotalFavouritesLength(int userId)
    {
        if (userId <= 0)
            return ResponseData<int>.Error("Invalid user ID.");

        return await _favouriteSongsRepository.GetTotalFavouritesLength(userId);
    }

    /// <summary>
    /// Получить все любимые песни для конкретного пользователя.
    /// </summary>
    public async Task<ResponseData<List<FavouriteSongDTO>>> GetFavouriteSongs(int userId)
    {
        if (userId <= 0)
            return ResponseData<List<FavouriteSongDTO>>.Error("Invalid user ID.");

        return await _favouriteSongsRepository.GetFavouriteSongs(userId);
    }

    /// <summary>
    /// Добавить песню в любимые.
    /// </summary>
    public async Task<ResponseData<int>> AddFavouriteSong(int userId, int songId)
    {
        if (userId <= 0)
            return ResponseData<int>.Error("Invalid user ID.");

        if (songId <= 0)
            return ResponseData<int>.Error("Invalid song ID.");

        return await _favouriteSongsRepository.AddFavouriteSong(userId, songId);
    }

    /// <summary>
    /// Удалить песню из любимых по ID.
    /// </summary>
    public async Task<ResponseData<int>> Delete(int favSongId)
    {
        if (favSongId <= 0)
            return ResponseData<int>.Error("Invalid song ID.");

        return await _favouriteSongsRepository.Delete(favSongId);
    }
}

