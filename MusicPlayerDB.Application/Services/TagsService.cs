using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class TagsService
{
    private readonly TagsRepository _tagsRepository;

    public TagsService(TagsRepository tagsRepository)
    {
        _tagsRepository = tagsRepository;
    }

    /// <summary>
    /// Добавление нового тега.
    /// </summary>
    public async Task<ResponseData<int>> AddTagAsync(string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
        {
            return ResponseData<int>.Error("Tag name cannot be empty.");
        }

        var newTag = new Tag { Name = tagName };
        return await _tagsRepository.Create(newTag);
    }

    /// <summary>
    /// Получение списка всех тегов.
    /// </summary>
    public async Task<ResponseData<List<Tag>>> GetAllTagsAsync()
    {
        return await _tagsRepository.GetAll();
    }

    /// <summary>
    /// Получение тега по идентификатору.
    /// </summary>
    public async Task<ResponseData<Tag>> GetTagByIdAsync(int id)
    {
        if (id <= 0)
        {
            return ResponseData<Tag>.Error("Invalid tag ID.");
        }

        return await _tagsRepository.GetById(id);
    }

    /// <summary>
    /// Обновление тега по идентификатору.
    /// </summary>
    public async Task<ResponseData<int>> UpdateTagAsync(int id, string newTagName)
    {
        if (id <= 0)
        {
            return ResponseData<int>.Error("Invalid tag ID.");
        }

        if (string.IsNullOrWhiteSpace(newTagName))
        {
            return ResponseData<int>.Error("Tag name cannot be empty.");
        }

        var updatedTag = new Tag { Id = id, Name = newTagName };
        return await _tagsRepository.Update(id, updatedTag);
    }

    /// <summary>
    /// Удаление тега по идентификатору.
    /// </summary>
    public async Task<ResponseData<int>> DeleteTagAsync(int id)
    {
        if (id <= 0)
        {
            return ResponseData<int>.Error("Invalid tag ID.");
        }

        return await _tagsRepository.Delete(id);
    }

    /// <summary>
    /// Получение списка тегов для песни.
    /// </summary>
    public async Task<ResponseData<List<SongWithTagsDTO>>> GetTagsForSong(int songId)
    {
        if (songId <= 0)
        {
            return ResponseData<List<SongWithTagsDTO>>.Error("Invalid song ID.");
        }

        try
        {
            return await _tagsRepository.GetTagsForSong(songId);
        }
        catch (Exception ex)
        {
            return ResponseData<List<SongWithTagsDTO>>.Error($"An error occurred: {ex.Message}");
        }
    }
}
