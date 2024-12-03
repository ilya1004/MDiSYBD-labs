using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class GenresService
{
    private readonly GenresRepository _genresRepository;

    public GenresService(GenresRepository genresRepository)
    {
        _genresRepository = genresRepository;
    }

    /// <summary>
    /// Создание нового жанра.
    /// </summary>
    public async Task<ResponseData<int>> Create(Genre genre)
    {
        if (genre == null)
            return ResponseData<int>.Error("Genre cannot be null.");

        if (string.IsNullOrWhiteSpace(genre.Name))
            return ResponseData<int>.Error("Genre name cannot be empty.");

        return await _genresRepository.Create(genre);
    }

    /// <summary>
    /// Получить все жанры.
    /// </summary>
    public async Task<ResponseData<List<Genre>>> GetAll()
    {
        return await _genresRepository.GetAll();
    }

    /// <summary>
    /// Получить жанр по ID.
    /// </summary>
    public async Task<ResponseData<Genre>> GetById(int id)
    {
        if (id <= 0)
            return ResponseData<Genre>.Error("Invalid genre ID.");

        return await _genresRepository.GetById(id);
    }

    /// <summary>
    /// Обновление информации о жанре.
    /// </summary>
    public async Task<ResponseData<int>> Update(int id, Genre genre)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid genre ID.");

        if (genre == null)
            return ResponseData<int>.Error("Genre cannot be null.");

        if (string.IsNullOrWhiteSpace(genre.Name))
            return ResponseData<int>.Error("Genre name cannot be empty.");

        return await _genresRepository.Update(id, genre);
    }

    /// <summary>
    /// Удаление жанра по ID.
    /// </summary>
    public async Task<ResponseData<int>> Delete(int id)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid genre ID.");

        return await _genresRepository.Delete(id);
    }
}
