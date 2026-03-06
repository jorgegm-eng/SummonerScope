namespace SummonerScope.Application.DTOs;

public class MatchAnalysisDto
{
    public string MatchId { get; set; } = string.Empty;
    public string Puuid { get; set; } = string.Empty;
    public string Champion { get; set; } = string.Empty;

    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }

    public double Kda { get; set; }

    public bool Win { get; set; }

    public int Gold { get; set; }
    public double GoldPerMinute { get; set; }

    public int Damage { get; set; }
    public double DamagePerMinute { get; set; }

    public int Cs { get; set; }
    public double CsPerMinute { get; set; }

    public int DurationSeconds { get; set; }
}