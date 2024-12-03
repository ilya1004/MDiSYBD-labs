using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class ArtistsService
{
    private readonly ArtistsRepository _artistsRepository;

    public ArtistsService(ArtistsRepository artistsRepository)
    {
        _artistsRepository = artistsRepository;
    }

    /// <summary>
    /// Получить список артистов с ограничением и смещением.
    /// </summary>
    public Task<ResponseData<List<ArtistDetailDTO>>> GetArtistsAsync(int limit = 10, int offset = 0)
    {
        return _artistsRepository.GetArtistsAsync(limit, offset);
    }

    /// <summary>
    /// Получить информацию об артисте по его ID.
    /// </summary>
    public Task<ResponseData<ArtistDetailDTO>> GetArtistByIdAsync(int artistId)
    {
        return _artistsRepository.GetArtistByIdAsync(artistId);
    }

    /// <summary>
    /// Получить артистов по количеству прослушиваний.
    /// </summary>
    public Task<ResponseData<List<ArtistPlaysCountDTO>>> GetArtistsByPlaysCount()
    {
        return _artistsRepository.GetArtistsByPlaysCount();
    }

}

