using SummonerScope.Infrastructure.RiotAPI.Models;

namespace SummonerScope.Infrastructure.RiotAPI;

public interface IRiotApiClient
{
    Task<RiotAccountResponse?> GetAccountByRiotIdAsync(string gameName, string tagLine);
    Task<List<string>?> GetMatchIdsByPuuidAsync(string puuid, int count = 10);
    Task<RiotMatchResponse?> GetMatchAsync(string matchId);
}