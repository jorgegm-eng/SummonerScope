using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using SummonerScope.Application.Interfaces;
using SummonerScope.Infrastructure.RiotAPI.Models;

namespace SummonerScope.Infrastructure.RiotAPI;

public class RiotApiClient : IRiotApiClient
{
    private readonly HttpClient _httpClient;
    private readonly RiotApiSettings _settings;
    private readonly ICacheService _cacheService;

    public RiotApiClient(HttpClient httpClient, IOptions<RiotApiSettings> options, ICacheService cacheService)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _cacheService = cacheService;
    }

    public async Task<RiotAccountResponse?> GetAccountByRiotIdAsync(string gameName, string tagLine)
    {
        var cacheKey = $"riot-account:{gameName}:{tagLine}".ToLowerInvariant();

        var cachedAccount = await _cacheService.GetAsync<RiotAccountResponse>(cacheKey);

        if (cachedAccount is not null)
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
            await _cacheService.SetAsync(cacheKey, account, TimeSpan.FromMinutes(30));
        }

        return account;
    }

    public async Task<List<string>?> GetMatchIdsByPuuidAsync(string puuid, int count = 10)
    {
        var cacheKey = $"riot-match-ids:{puuid}:{count}".ToLowerInvariant();

        var cachedMatchIds = await _cacheService.GetAsync<List<string>>(cacheKey);

        if (cachedMatchIds is not null)
        {
            return cachedMatchIds;
        }

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

        var matchIds = await response.Content.ReadFromJsonAsync<List<string>>();

        if (matchIds is not null)
        {
            await _cacheService.SetAsync(cacheKey, matchIds, TimeSpan.FromMinutes(10));
        }

        return matchIds;
    }

    public async Task<RiotMatchResponse?> GetMatchAsync(string matchId)
    {
        var cacheKey = $"riot-match:{matchId}".ToLowerInvariant();

        var cachedMatch = await _cacheService.GetAsync<RiotMatchResponse>(cacheKey);

        if (cachedMatch is not null)
        {
            return cachedMatch;
        }

        var url = $"https://europe.api.riotgames.com/lol/match/v5/matches/{matchId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Riot-Token", _settings.ApiKey);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var match = await response.Content.ReadFromJsonAsync<RiotMatchResponse>();

        if (match is not null)
        {
            await _cacheService.SetAsync(cacheKey, match, TimeSpan.FromMinutes(15));
        }

        return match;
    }
}