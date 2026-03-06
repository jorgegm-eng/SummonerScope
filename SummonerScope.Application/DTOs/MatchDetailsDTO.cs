namespace SummonerScope.Application.DTOs;

public class MatchDetailsDto
{
    public string MatchId { get; set; } = string.Empty;

    public string Champion { get; set; } = string.Empty;

    public int Kills { get; set; }

    public int Deaths { get; set; }

    public int Assists { get; set; }

    public bool Win { get; set; }

    public int Gold { get; set; }

    public int Damage { get; set; }

    public int Cs { get; set; }

    public int Duration { get; set; }
}