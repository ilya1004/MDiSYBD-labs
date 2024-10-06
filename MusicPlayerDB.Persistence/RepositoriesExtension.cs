using Microsoft.Extensions.DependencyInjection;
using MusicPlayerDB.Persistence.Repositories;

namespace MusicPlayerDB.Persistence;

public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {

        services.AddScoped<TagsRepository>();

        return services;
    }
}
