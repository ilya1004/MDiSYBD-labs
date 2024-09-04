using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.Persistence.Models;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SongsController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetByName(string name)
    {
        return Results.Json(new { });
    }

    [HttpGet]
    public async Task<IResult> GetByArtist(string name)
    {
        return Results.Json(new { });
    }

    [HttpGet]
    public async Task<IResult> GetByAlbum(string title)
    {
        return Results.Json(new { });
    }

    [HttpGet]
    public async Task<IResult> GetByGenre(string name, int limit)
    {
        return Results.Json(new { });
    }

    [HttpGet]
    public async Task<IResult> GetByTag(string name, int limit)
    {
        return Results.Json(new { });
    }
}
