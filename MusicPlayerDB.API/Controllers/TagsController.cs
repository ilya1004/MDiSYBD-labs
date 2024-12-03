using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Services;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[EnableCors("myCorsPolicy")]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly TagsService _tagsService;

    public TagsController(TagsService tagsService)
    {
        _tagsService = tagsService;
    }

    /// <summary>
    /// Получить все теги.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetAllTags()
    {
        var response = await _tagsService.GetAllTagsAsync();
        if (response.Successfull)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.ErrorMessage);
    }

    /// <summary>
    /// Получить тег по идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> GetTagById(int id)
    {
        var response = await _tagsService.GetTagByIdAsync(id);
        if (response.Successfull)
        {
            return Ok(response.Data);
        }

        return NotFound(response.ErrorMessage);
    }

    /// <summary>
    /// Добавить новый тег.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> AddTag([FromBody] string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
        {
            return BadRequest("Tag name cannot be empty.");
        }

        var response = await _tagsService.AddTagAsync(tagName);
        if (response.Successfull)
        {
            return CreatedAtAction(nameof(GetTagById), new { id = response.Data }, response.Data);
        }

        return BadRequest(response.ErrorMessage);
    }

    /// <summary>
    /// Обновить тег по идентификатору.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPolicy)]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] string newTagName)
    {
        if (string.IsNullOrWhiteSpace(newTagName))
        {
            return BadRequest("Tag name cannot be empty.");
        }

        var response = await _tagsService.UpdateTagAsync(id, newTagName);
        if (response.Successfull)
        {
            return Ok(response.Data);
        }

        return NotFound(response.ErrorMessage);
    }

    /// <summary>
    /// Удалить тег по идентификатору.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.UserAndArtistAndAdminPolicy)]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var response = await _tagsService.DeleteTagAsync(id);
        if (response.Successfull)
        {
            return Ok(response.Data);
        }

        return NotFound(response.ErrorMessage);
    }
}
