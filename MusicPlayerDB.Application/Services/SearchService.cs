using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class SearchService
{
    private readonly SearchRepository _searchRepository;
    public SearchService(SearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }

    /// <summary>
    /// Получить результат по поиску.
    /// </summary>
    public async Task<ResponseData<List<SearchItemDTO>>> MakeSearch(string query)
    {
        return await _searchRepository.MakeSearch(query);
    }

}
