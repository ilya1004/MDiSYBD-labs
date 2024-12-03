using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class AlbumsService
{
    private readonly AlbumsRepository _albumsRepository;

    public AlbumsService(AlbumsRepository albumsRepository)
    {
        _albumsRepository = albumsRepository;
    }

    /// <summary>
    /// Создание нового альбома.
    /// </summary>
    public async Task<ResponseData<int>> Create(Album album)
    {
        if (album == null)
            return ResponseData<int>.Error("Album cannot be null.");

        return await _albumsRepository.Create(album);
    }

    /// <summary>
    /// Получить все альбомы.
    /// </summary>
    public async Task<ResponseData<List<Album>>> GetAll()
    {
        return await _albumsRepository.GetAll();
    }

    /// <summary>
    /// Получить все альбомы по ID артиста.
    /// </summary>
    public async Task<ResponseData<List<Album>>> GetByArtistId(int artistId)
    {
        return await _albumsRepository.GetByArtistId(artistId);
    }

    /// <summary>
    /// Получить альбом по ID.
    /// </summary>
    public async Task<ResponseData<Album>> GetById(int id)
    {
        if (id <= 0)
            return ResponseData<Album>.Error("Invalid album ID.");

        return await _albumsRepository.GetById(id);
    }

    /// <summary>
    /// Обновление информации об альбоме.
    /// </summary>
    public async Task<ResponseData<int>> Update(int id, Album album)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid album ID.");

        if (album == null)
            return ResponseData<int>.Error("Album cannot be null.");

        return await _albumsRepository.Update(id, album);
    }

    /// <summary>
    /// Удаление альбома по ID.
    /// </summary>
    public async Task<ResponseData<int>> Delete(int id)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid album ID.");

        return await _albumsRepository.Delete(id);
    }
}

