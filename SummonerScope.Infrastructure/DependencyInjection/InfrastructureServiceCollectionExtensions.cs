using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SummonerScope.Application.Interfaces;
using SummonerScope.Infrastructure.Data;
using SummonerScope.Infrastructure.RiotAPI;

namespace SummonerScope.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RiotApiSettings>(configuration.GetSection(RiotApiSettings.SectionName));

        services.AddHttpClient<IRiotApiClient, RiotApiClient>();

        services.AddScoped<ICacheService, MemoryCacheService>();

        services.AddScoped<ICacheService, RedisCacheService>();
        
        services.AddScoped<IMatchAnalyzer, MatchAnalyzer>();

        services.AddScoped<IPlayerService, PlayerService>();

        return services;
    }
}