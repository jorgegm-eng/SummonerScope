using SummonerScope.Application.DTOs;
using SummonerScope.Application.Interfaces;

namespace SummonerScope.Infrastructure.RiotAPI;

public class MatchAnalyzer : IMatchAnalyzer
{
    public MatchAnalysisDto AnalyzeMatch(MatchParticipantStatsDto stats)
    {
        var durationMinutes = stats.DurationSeconds / 60.0;
        var cs = stats.LaneMinions + stats.JungleMinions;

        var kda = stats.Deaths == 0
            ? stats.Kills + stats.Assists
            : (double)(stats.Kills + stats.Assists) / stats.Deaths;

        return new MatchAnalysisDto
        {
            MatchId = stats.MatchId,
            Puuid = stats.Puuid,
            Champion = stats.ChampionName,
            Kills = stats.Kills,
            Deaths = stats.Deaths,
            Assists = stats.Assists,
            Kda = Math.Round(kda, 2),
            Win = stats.Win,
            Gold = stats.GoldEarned,
            GoldPerMinute = durationMinutes > 0 ? Math.Round(stats.GoldEarned / durationMinutes, 2) : 0,
            Damage = stats.Damage,
            DamagePerMinute = durationMinutes > 0 ? Math.Round(stats.Damage / durationMinutes, 2) : 0,
            Cs = cs,
            CsPerMinute = durationMinutes > 0 ? Math.Round(cs / durationMinutes, 2) : 0,
            DurationSeconds = stats.DurationSeconds
        };
    }

    public PlayerMatchAnalysisDto AnalyzePlayerMatch(MatchParticipantStatsDto stats)
    {
        var durationMinutes = stats.DurationSeconds / 60.0;
        var cs = stats.LaneMinions + stats.JungleMinions;

        var kda = stats.Deaths == 0
            ? stats.Kills + stats.Assists
            : (double)(stats.Kills + stats.Assists) / stats.Deaths;

        return new PlayerMatchAnalysisDto
        {
            MatchId = stats.MatchId,
            Champion = stats.ChampionName,
            Kda = Math.Round(kda, 2),
            CsPerMinute = durationMinutes > 0 ? Math.Round(cs / durationMinutes, 2) : 0,
            DamagePerMinute = durationMinutes > 0 ? Math.Round(stats.Damage / durationMinutes, 2) : 0,
            Win = stats.Win
        };
    }
}