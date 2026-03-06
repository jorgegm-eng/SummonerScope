using SummonerScope.Application.DTOs;
using SummonerScope.Application.Interfaces;

namespace SummonerScope.Infrastructure.RiotAPI;

public class PlayerService : IPlayerService
{
    private readonly IRiotApiClient _riotApiClient;

    public PlayerService(IRiotApiClient riotApiClient)
    {
        _riotApiClient = riotApiClient;
    }

    public async Task<PlayerDto?> GetPlayerAsync(string region, string gameName, string tagLine)
    {
        var account = await _riotApiClient.GetAccountByRiotIdAsync(gameName, tagLine);

        if (account is null)
        {
            return null;
        }

        return new PlayerDto
        {
            Region = region,
            GameName = account.GameName,
            TagLine = account.TagLine,
            Puuid = account.Puuid
        };
    }

    public async Task<PlayerMatchesDto?> GetPlayerMatchesAsync(string region, string gameName, string tagLine, int count = 10)
    {
        var account = await _riotApiClient.GetAccountByRiotIdAsync(gameName, tagLine);

        if (account is null)
        {
            return null;
        }

        var matchIds = await _riotApiClient.GetMatchIdsByPuuidAsync(account.Puuid, count);

        if (matchIds is null)
        {
            return null;
        }

        return new PlayerMatchesDto
        {
            Region = region,
            GameName = account.GameName,
            TagLine = account.TagLine,
            MatchIds = matchIds
        };
    }
}