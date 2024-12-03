using Microsoft.Extensions.DependencyInjection;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Persistence.Extensions;

public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<AlbumsRepository>();
        services.AddScoped<ArtistsRepository>();
        services.AddScoped<EventsRepository>();
        services.AddScoped<FavouriteSongsRepository>();
        services.AddScoped<GenresRepository>();
        services.AddScoped<LogsRepository>();
        services.AddScoped<PlaylistsRepository>();
        services.AddScoped<ReviewsRepository>();
        services.AddScoped<SearchRepository>();
        services.AddScoped<SongsRepository>();
        services.AddScoped<TagsRepository>();
        services.AddScoped<UsersRepository>();

        return services;
    }
}
