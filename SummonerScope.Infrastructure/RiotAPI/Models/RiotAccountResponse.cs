using System.Text.Json.Serialization;

namespace SummonerScope.Infrastructure.RiotAPI.Models;

public class RiotAccountResponse
{
    [JsonPropertyName("puuid")]
    public string Puuid { get; set; } = string.Empty;

    [JsonPropertyName("gameName")]
    public string GameName { get; set; } = string.Empty;

    [JsonPropertyName("tagLine")]
    public string TagLine { get; set; } = string.Empty;
}