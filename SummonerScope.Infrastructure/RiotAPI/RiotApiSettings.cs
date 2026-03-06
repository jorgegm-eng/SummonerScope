namespace SummonerScope.Infrastructure.RiotAPI;

public class RiotApiSettings
{
    public const string SectionName = "RiotApi";

    public string ApiKey { get; set; } = string.Empty;
    public string AccountBaseUrl { get; set; } = string.Empty;
}