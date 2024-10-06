using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.Persistence;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdminController : ControllerBase
{
    private readonly MusicPlayerDbContext _context;
    public AdminController(MusicPlayerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IResult> GetSex()
    {
        var str = _context.Database.CanConnectAsync();

        return Results.Ok(str);
    }
}
