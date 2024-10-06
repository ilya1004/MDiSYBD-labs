using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.Persistence;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController : ControllerBase
{
    private readonly MusicPlayerDbContext _context;
    public GenresController(MusicPlayerDbContext context) 
    {
        _context = context;    
    }

    [HttpGet]
    public async Task<IResult> GetAll()
    {


        return Results.Ok("qwe");
    }

    [HttpPost]
    public async Task<IResult> PostAll()
    {
        return Results.Ok("123");
    }
}
