using SummonerScope.Application.DTOs;
using SummonerScope.Application.Interfaces;
using SummonerScope.Infrastructure.RiotAPI.Models;

namespace SummonerScope.Infrastructure.RiotAPI;

public class MatchAnalyzer : IMatchAnalyzer
{
    public MatchAnalysisDto? AnalyzeMatch(RiotMatchResponse match, string puuid)
    {
        var participant = match.Info.Participants.FirstOrDefault(p => p.Puuid == puuid);

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
            Cs = cs,
            CsPerMinute = durationMinutes > 0 ? Math.Round(cs / durationMinutes, 2) : 0,
            DurationSeconds = durationSeconds
        };
    }

    public PlayerMatchAnalysisDto? AnalyzePlayerMatch(RiotMatchResponse match, string puuid)
    {
        var participant = match.Info.Participants.FirstOrDefault(p => p.Puuid == puuid);

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
    }
}