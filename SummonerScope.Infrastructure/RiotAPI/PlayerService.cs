using SummonerScope.Application.DTOs;
using SummonerScope.Application.Interfaces;

namespace SummonerScope.Infrastructure.RiotAPI;

public class PlayerService : IPlayerService
{
    private readonly IRiotApiClient _riotApiClient;
    private readonly IMatchAnalyzer _matchAnalyzer;

    public PlayerService(IRiotApiClient riotApiClient, IMatchAnalyzer matchAnalyzer)
    {
        _riotApiClient = riotApiClient;
        _matchAnalyzer = matchAnalyzer;
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

    public async Task<MatchAnalysisDto?> GetMatchAnalysisAsync(string matchId, string puuid)
    {
        var match = await _riotApiClient.GetMatchAsync(matchId);

        if (match is null)
        {
            return null;
        }

        var stats = MapParticipantStats(match, puuid);

        if (stats is null)
        {
            return null;
        }

        return _matchAnalyzer.AnalyzeMatch(stats);
    }

    public async Task<List<PlayerMatchAnalysisDto>?> GetPlayerMatchAnalysisAsync(string region, string gameName, string tagLine, int count = 10)
    {
        var account = await _riotApiClient.GetAccountByRiotIdAsync(gameName, tagLine);

        if (account is null)
        {
            return null;
        }

        var matchIds = await _riotApiClient.GetMatchIdsByPuuidAsync(account.Puuid, count);

        if (matchIds is null || matchIds.Count == 0)
        {
            return null;
        }

        var matchAnalysisTasks = matchIds.Select(async matchId =>
        {
            var match = await _riotApiClient.GetMatchAsync(matchId);

            if (match is null)
            {
                return null;
            }

            var stats = MapParticipantStats(match, account.Puuid);

            if (stats is null)
            {
                return null;
            }

            return _matchAnalyzer.AnalyzePlayerMatch(stats);
        });

        var results = await Task.WhenAll(matchAnalysisTasks);

        return results
            .Where(x => x is not null)
            .Cast<PlayerMatchAnalysisDto>()
            .ToList();
    }

    private static MatchParticipantStatsDto? MapParticipantStats(SummonerScope.Infrastructure.RiotAPI.Models.RiotMatchResponse match, string puuid)
    {
        var participant = match.Info.Participants.FirstOrDefault(p => p.Puuid == puuid);

        if (participant is null)
        {
            return null;
        }

        return new MatchParticipantStatsDto
        {
            MatchId = match.Metadata.MatchId,
            Puuid = participant.Puuid,
            ChampionName = participant.ChampionName,
            Kills = participant.Kills,
            Deaths = participant.Deaths,
            Assists = participant.Assists,
            Win = participant.Win,
            GoldEarned = participant.GoldEarned,
            Damage = participant.Damage,
            LaneMinions = participant.LaneMinions,
            JungleMinions = participant.JungleMinions,
            DurationSeconds = match.Info.GameDuration
        };
    }
}