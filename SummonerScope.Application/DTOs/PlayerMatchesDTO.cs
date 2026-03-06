namespace SummonerScope.Application.DTOs;

public class PlayerMatchesDto
{
    public string GameName { get; set; } = string.Empty;
    public string TagLine { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public List<string> MatchIds { get; set; } = [];
}