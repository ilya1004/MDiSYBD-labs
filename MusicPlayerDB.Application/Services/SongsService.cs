using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class SongsService
{
    private readonly SongsRepository _songRepository;

    public SongsService(SongsRepository songRepository)
    {
        _songRepository = songRepository;
    }

    /// <summary>
    /// Получение всех песен
    /// </summary>
    public async Task<ResponseData<List<Song>>> GetAllSongsAsync()
    {
        return await _songRepository.GetAllSongsAsync();
    }

    /// <summary>
    /// Получение песни по ID
    /// </summary>
    public async Task<ResponseData<Song>> GetSongByIdAsync(int id)
    {
        return await _songRepository.GetSongByIdAsync(id);
    }

    /// <summary>
    /// Добавление новой песни
    /// </summary>
    public async Task<ResponseData<bool>> AddSongAsync(Song song)
    {
        return await _songRepository.AddSongAsync(song);
    }

    /// <summary>
    /// Обновление песни
    /// </summary>
    public async Task<ResponseData<bool>> UpdateSongAsync(int id, Song updatedSong)
    {
        return await _songRepository.UpdateSongAsync(id, updatedSong);
    }

    /// <summary>
    /// Удаление песни
    /// </summary>
    public async Task<ResponseData<bool>> DeleteSongAsync(int id)
    {
        return await _songRepository.DeleteSongAsync(id);
    }

    /// <summary>
    /// Получение списка песен с их средними оценками.
    /// </summary>
    public async Task<ResponseData<List<SongAverageRatingDTO>>> GetAverageSongRating(int limit = 20, int offset = 0)
    {
        if (limit <= 0)
        {
            return ResponseData<List<SongAverageRatingDTO>>.Error("Limit must be greater than zero.");
        }

        if (offset < 0)
        {
            return ResponseData<List<SongAverageRatingDTO>>.Error("Offset cannot be negative.");
        }

        try
        {
            return await _songRepository.GetAverageSongRating(limit, offset);
        }
        catch (Exception ex)
        {
            return ResponseData<List<SongAverageRatingDTO>>.Error($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Получение списка песен по идентификатору исполнителя.
    /// </summary>
    public async Task<ResponseData<List<Song>>> GetSongsByArtist(int artistId)
    {
        if (artistId <= 0)
        {
            return ResponseData<List<Song>>.Error("Invalid artist ID.");
        }

        try
        {
            return await _songRepository.GetByArtistId(artistId);
        }
        catch (Exception ex)
        {
            return ResponseData<List<Song>>.Error($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Получение списка песен по идентификатору альбома.
    /// </summary>
    public async Task<ResponseData<List<Song>>> GetByAlbumId(int albumId)
    {
        if (albumId <= 0)
        {
            return ResponseData<List<Song>>.Error("Invalid album ID.");
        }

        try
        {
            return await _songRepository.GetByAlbumId(albumId);
        }
        catch (Exception ex)
        {
            return ResponseData<List<Song>>.Error($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Получение списка песен по идентификатору плейлиста.
    /// </summary>
    public async Task<ResponseData<List<SongByPlaylistDTO>>> GetSongsByPlaylist(int playlistId)
    {
        if (playlistId <= 0)
        {
            return ResponseData<List<SongByPlaylistDTO>>.Error("Invalid playlist ID.");
        }

        try
        {
            return await _songRepository.GetSongsByPlaylist(playlistId);
        }
        catch (Exception ex)
        {
            return ResponseData<List<SongByPlaylistDTO>>.Error($"An error occurred: {ex.Message}");
        }
    }


    public async Task<ResponseData<List<SongInPlaylistDTO>>> GetSongsInPlaylistByUser(int userId)
    {
        if (userId <= 0)
        {
            return ResponseData<List<SongInPlaylistDTO>>.Error("Invalid playlist ID.");
        }

        try
        {
            return await _songRepository.GetSongsInPlaylistByUser(userId);
        }
        catch (Exception ex)
        {
            return ResponseData<List<SongInPlaylistDTO>>.Error($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Получение списка песен по жанру.
    /// </summary>
    public async Task<ResponseData<List<SongDetailDTO>>> GetSongsByGenre(int genreId)
    {
        if (genreId <= 0)
        {
            return ResponseData<List<SongDetailDTO>>.Error("Invalid genre ID.");
        }

        try
        {
            return await _songRepository.GetSongsByGenre(genreId);
        }
        catch (Exception ex)
        {
            return ResponseData<List<SongDetailDTO>>.Error($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Получение списка песен по тегу.
    /// </summary>
    public async Task<ResponseData<List<SongDetailDTO>>> GetSongsByTag(int tagId)
    {
        if (tagId <= 0)
        {
            return ResponseData<List<SongDetailDTO>>.Error("Invalid tag ID.");
        }

        try
        {
            return await _songRepository.GetSongsByTag(tagId);
        }
        catch (Exception ex)
        {
            return ResponseData<List<SongDetailDTO>>.Error($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Обновление количества воспроизведений песни.
    /// </summary>
    public async Task<ResponseData<bool>> UpdateSongPlaysCount(int songId, int count)
    {
        if (songId <= 0)
        {
            return ResponseData<bool>.Error("Invalid song ID.");
        }

        if (count <= 0)
        {
            return ResponseData<bool>.Error("Plays count must be greater than zero.");
        }

        try
        {
            return await _songRepository.UpdateSongPlaysCount(songId, count);
        }
        catch (Exception ex)
        {
            return ResponseData<bool>.Error($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Добавление песни в избранное.
    /// </summary>
    public async Task<ResponseData<bool>> AddSongToFavourite(int songId, int userId)
    {
        if (songId <= 0)
        {
            return ResponseData<bool>.Error("Invalid song ID.");
        }

        if (userId <= 0)
        {
            return ResponseData<bool>.Error("Invalid user ID.");
        }

        try
        {
            return await _songRepository.AddSongToFavourite(songId, userId);
        }
        catch (Exception ex)
        {
            return ResponseData<bool>.Error($"An error occurred: {ex.Message}");
        }
    }
}


