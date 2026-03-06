namespace SummonerScope.Application.DTOs;

public class MatchParticipantStatsDto
{
    public string MatchId { get; set; } = string.Empty;
    public string Puuid { get; set; } = string.Empty;
    public string ChampionName { get; set; } = string.Empty;

    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }

    public bool Win { get; set; }

    public int GoldEarned { get; set; }
    public int Damage { get; set; }

    public int LaneMinions { get; set; }
    public int JungleMinions { get; set; }

    public int DurationSeconds { get; set; }
}