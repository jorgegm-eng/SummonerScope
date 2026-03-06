namespace SummonerScope.Application.DTOs;

public class PlayerMatchAnalysisDto
{
    public string MatchId { get; set; } = string.Empty;

    public string Champion { get; set; } = string.Empty;

    public double Kda { get; set; }

    public double CsPerMinute { get; set; }

    public double DamagePerMinute { get; set; }

    public bool Win { get; set; }
}