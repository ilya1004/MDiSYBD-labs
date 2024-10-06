using Microsoft.EntityFrameworkCore;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Domain.Utils;

namespace MusicPlayerDB.Persistence.Repositories;

public class TagsRepository
{
    private readonly MusicPlayerDbContext _context;
    public TagsRepository(MusicPlayerDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseData<int>> Create(Tag tag)
    {
        try
        {
            var rows = await _context.Database.ExecuteSqlAsync($"INSERT INTO tags (name) VALUES ({tag.Name})");
            return ResponseData<int>.Success(rows);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
            return ResponseData<int>.Error(ex.Message);
        }
    }

    public ResponseData<List<Tag>> GetAll()
    {
        var data = _context.Database.SqlQuery<Tag>($"SELECT * FROM tags ORDER BY id");

        return ResponseData<List<Tag>>.Success(data.ToList());
    }

    public ResponseData<List<Tag>> GetById(int id)
    {
        var data = _context.Database.SqlQuery<Tag>($"SELECT * FROM tags WHERE id = {id}");

        return ResponseData<List<Tag>>.Success(data.ToList());
    }

    public async Task<ResponseData<int>> Update(int id, Tag tag)
    {
        try
        {
            var rows = await _context.Database.ExecuteSqlAsync($"UPDATE tags SET name = {tag.Name} WHERE id = {id}");
            return ResponseData<int>.Success(rows);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
            return ResponseData<int>.Error(ex.Message);
        }
    }

    public async Task<ResponseData<int>> Delete(int id)
    {
        try
        {
            var rows = await _context.Database.ExecuteSqlAsync($"DELETE FROM tags WHERE id = {id}");
            return ResponseData<int>.Success(rows);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
            return ResponseData<int>.Error(ex.Message);
        }
    }
}
