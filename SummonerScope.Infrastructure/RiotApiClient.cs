using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SummonerScope.Infrastructure.RiotAPI.Models;

namespace SummonerScope.Infrastructure.RiotAPI;

public class RiotApiClient : IRiotApiClient
{
    private readonly HttpClient _httpClient;
    private readonly RiotApiSettings _settings;
    private readonly IMemoryCache _cache;

    public RiotApiClient(HttpClient httpClient, IOptions<RiotApiSettings> options, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _cache = cache;
    }

    public async Task<RiotAccountResponse?> GetAccountByRiotIdAsync(string gameName, string tagLine)
    {
        var cacheKey = $"riot-account:{gameName}:{tagLine}".ToLowerInvariant();

        if (_cache.TryGetValue(cacheKey, out RiotAccountResponse? cachedAccount))
        {
            return cachedAccount;
        }

        var url =
            $"{_settings.AccountBaseUrl}/riot/account/v1/accounts/by-riot-id/{Uri.EscapeDataString(gameName)}/{Uri.EscapeDataString(tagLine)}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Riot-Token", _settings.ApiKey);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var account = await response.Content.ReadFromJsonAsync<RiotAccountResponse>();

        if (account is not null)
        {
            _cache.Set(cacheKey, account, TimeSpan.FromMinutes(30));
        }

        return account;
    }

    public async Task<List<string>?> GetMatchIdsByPuuidAsync(string puuid, int count = 10)
    {
        var url =
            $"https://europe.api.riotgames.com/lol/match/v5/matches/by-puuid/{Uri.EscapeDataString(puuid)}/ids?start=0&count={count}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Riot-Token", _settings.ApiKey);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<string>>();
    }

    public async Task<RiotMatchResponse?> GetMatchAsync(string matchId)
    {
        var url = $"https://europe.api.riotgames.com/lol/match/v5/matches/{matchId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Riot-Token", _settings.ApiKey);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<RiotMatchResponse>();
    }
}