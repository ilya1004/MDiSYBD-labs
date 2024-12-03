using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;

namespace MusicPlayerDB.API.Controllers;


[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;
    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }

    
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> MakeSearch(string query)
    {
        var result = await _searchService.MakeSearch(query);

        if (result.Successfull)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

}
