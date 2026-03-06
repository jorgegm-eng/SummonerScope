namespace SummonerScope.Domain.Entities;

public class Player
{
    public Guid Id { get; set; }
    public string Puuid { get; set; } = string.Empty;
    public string GameName { get; set; } = string.Empty;
    public string TagLine { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public DateTime LastUpdatedUtc { get; set; }
}