using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.DTOs;
using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using System.Runtime.CompilerServices;

namespace MusicPlayerDB.Persistence.Repositories;

public class SearchRepository
{
    private readonly MusicPlayerDbContext _context;

    public SearchRepository(MusicPlayerDbContext context)
    {
        _context = context;
    }

    private async Task<ResponseData<T>> ExecuteWithErrorHandling<T>(Func<Task<T>> func)
    {
        try
        {
            return ResponseData<T>.Success(await func());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
            return ResponseData<T>.Error(ex.Message);
        }
    }

    /// <summary>
    /// Получить результаты по поиску.
    /// </summary>
    public Task<ResponseData<List<SearchItemDTO>>> MakeSearch(string query)
    {
        return ExecuteWithErrorHandling(async () =>
        {
            var data = await _context.Database.SqlQuery<SearchItemDTO>(
                FormattableStringFactory.Create(@$"
                    select s.id, s.title name, 'song' type from songs s
                    where s.title ilike '%{query}%'
                    union all
                    select u.id, ai.name name, 'artist' type from users u
                    inner join artist_info ai on u.artist_info_id = ai.id 
                    where ai.name ilike '%{query}%'
                    union all
                    select p.id, p.title name, 'playlist' type from playlists p
                    where p.title ilike '%{query}%' and is_public = true
                    union all
                    select a.id, a.title name, 'album' type from albums a
                    where a.title ilike '%{query}%'
                    order by type, name;")).AsNoTracking().ToListAsync();

            return data;
        });
    }

}
