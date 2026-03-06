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

    public async Task<MatchAnalysisDto?> GetMatchAnalysisAsync(string matchId, string puuid)
    {
        var match = await _riotApiClient.GetMatchAsync(matchId);

        if (match is null)
        {
            return null;
        }

        var participant = match.Info.Participants.FirstOrDefault(p => p.Puuid == puuid);

        if (participant is null)
        {
            return null;
        }

        var durationSeconds = match.Info.GameDuration;
        var durationMinutes = durationSeconds / 60.0;

        var kda = participant.Deaths == 0
            ? participant.Kills + participant.Assists
            : (double)(participant.Kills + participant.Assists) / participant.Deaths;

        return new MatchAnalysisDto
        {
            MatchId = match.Metadata.MatchId,
            Puuid = participant.Puuid,
            Champion = participant.ChampionName,
            Kills = participant.Kills,
            Deaths = participant.Deaths,
            Assists = participant.Assists,
            Kda = Math.Round(kda, 2),
            Win = participant.Win,
            Gold = participant.GoldEarned,
            GoldPerMinute = durationMinutes > 0 ? Math.Round(participant.GoldEarned / durationMinutes, 2) : 0,
            Damage = participant.Damage,
            DamagePerMinute = durationMinutes > 0 ? Math.Round(participant.Damage / durationMinutes, 2) : 0,
            Cs = participant.LaneMinions + participant.JungleMinions,
            CsPerMinute = durationMinutes > 0 ? Math.Round((participant.LaneMinions + participant.JungleMinions) / durationMinutes, 2) : 0,
            DurationSeconds = durationSeconds
        };
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

            var participant = match.Info.Participants.FirstOrDefault(p => p.Puuid == account.Puuid);

            if (participant is null)
            {
                return null;
            }

            var durationSeconds = match.Info.GameDuration;
            var durationMinutes = durationSeconds / 60.0;

            var cs = participant.LaneMinions + participant.JungleMinions;

            var kda = participant.Deaths == 0
                ? participant.Kills + participant.Assists
                : (double)(participant.Kills + participant.Assists) / participant.Deaths;

            return new PlayerMatchAnalysisDto
            {
                MatchId = match.Metadata.MatchId,
                Champion = participant.ChampionName,
                Kda = Math.Round(kda, 2),
                CsPerMinute = durationMinutes > 0 ? Math.Round(cs / durationMinutes, 2) : 0,
                DamagePerMinute = durationMinutes > 0 ? Math.Round(participant.Damage / durationMinutes, 2) : 0,
                Win = participant.Win
            };
        });

        var results = await Task.WhenAll(matchAnalysisTasks);

        return results
            .Where(x => x is not null)
            .Cast<PlayerMatchAnalysisDto>()
            .ToList();
    }
}