using Microsoft.Extensions.DependencyInjection;
using MusicPlayerDB.Application.Services;

namespace MusicPlayerDB.Application.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<AlbumsService>();
        services.AddScoped<ArtistsService>();
        services.AddScoped<EventsService>();
        services.AddScoped<FavouriteSongsService>();
        services.AddScoped<GenresService>();
        services.AddScoped<LogsService>();
        services.AddScoped<PlaylistsService>();
        services.AddScoped<ReviewsService>();
        services.AddScoped<SearchService>();
        services.AddScoped<SongsService>();
        services.AddScoped<TagsService>();
        services.AddScoped<UsersService>();

        return services;
    }
}
