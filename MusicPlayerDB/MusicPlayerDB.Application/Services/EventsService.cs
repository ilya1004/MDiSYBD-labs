using MusicPlayerDB.Domain.Entities;
using MusicPlayerDB.Domain.Models;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Application.Services;

public class EventsService
{
    private readonly EventsRepository _eventsRepository;

    public EventsService(EventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    /// <summary>
    /// Создание нового события.
    /// </summary>
    public async Task<ResponseData<int>> Create(Event eventItem)
    {
        if (eventItem == null)
            return ResponseData<int>.Error("Event cannot be null.");

        if (string.IsNullOrEmpty(eventItem.Title))
            return ResponseData<int>.Error("Event title cannot be empty.");

        if (string.IsNullOrEmpty(eventItem.Location))
            return ResponseData<int>.Error("Event location cannot be empty.");

        if (eventItem.DateTime == default)
            return ResponseData<int>.Error("Event date and time must be specified.");

        return await _eventsRepository.Create(eventItem);
    }

    /// <summary>
    /// Получение всех событий.
    /// </summary>
    public async Task<ResponseData<List<Event>>> GetAll()
    {
        return await _eventsRepository.GetAll();
    }

    /// <summary>
    /// Получение события по ID.
    /// </summary>
    public async Task<ResponseData<Event>> GetById(int id)
    {
        if (id <= 0)
            return ResponseData<Event>.Error("Invalid event ID.");

        return await _eventsRepository.GetById(id);
    }

    /// <summary>
    /// Обновление информации о событии.
    /// </summary>
    public async Task<ResponseData<int>> Update(int id, Event eventItem)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid event ID.");

        if (eventItem == null)
            return ResponseData<int>.Error("Event cannot be null.");

        if (string.IsNullOrEmpty(eventItem.Title))
            return ResponseData<int>.Error("Event title cannot be empty.");

        if (string.IsNullOrEmpty(eventItem.Location))
            return ResponseData<int>.Error("Event location cannot be empty.");

        if (eventItem.DateTime == default)
            return ResponseData<int>.Error("Event date and time must be specified.");

        return await _eventsRepository.Update(id, eventItem);
    }

    /// <summary>
    /// Удаление события по ID.
    /// </summary>
    public async Task<ResponseData<int>> Delete(int id)
    {
        if (id <= 0)
            return ResponseData<int>.Error("Invalid event ID.");

        return await _eventsRepository.Delete(id);
    }

    /// <summary>
    /// Получение событий по ID артиста.
    /// </summary>
    public async Task<ResponseData<List<Event>>> GetByArtistId(int artistId)
    {
        if (artistId <= 0)
            return ResponseData<List<Event>>.Error("Invalid artist ID.");

        return await _eventsRepository.GetByArtistId(artistId);
    }
}

