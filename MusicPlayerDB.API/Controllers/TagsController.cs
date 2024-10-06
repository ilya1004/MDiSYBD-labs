using Microsoft.AspNetCore.Mvc;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Domain.Utils;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TagsController : ControllerBase
{
    private readonly TagsRepository _tagsRepository;
    public TagsController(TagsRepository tagsRepository)
    {
        _tagsRepository = tagsRepository;
    }

    [HttpPost]
    public async Task<IResult> Create(Tag tag)
    {
        var response = await _tagsRepository.Create(tag);

        return Results.Ok(response);
    }

    [HttpGet]
    public IResult GetAll()
    {
        var response = _tagsRepository.GetAll();

        return Results.Ok(response);
    }

    [HttpGet]
    public IResult GetById(int id)
    {
        var response = _tagsRepository.GetById(id);

        return Results.Ok(response);
    }

    [HttpPut]
    public async Task<IResult> Update(int id, Tag tag)
    {
        var response = await _tagsRepository.Update(id, tag);

        return Results.Ok(response);
    }

    [HttpDelete]
    public async Task<IResult> Delete(int id)
    {
        var response = await _tagsRepository.Delete(id);

        return Results.Ok(response);
    }
}
