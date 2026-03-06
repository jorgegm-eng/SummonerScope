using System.Text.Json.Serialization;

namespace SummonerScope.Infrastructure.RiotAPI.Models;

public class RiotMatchResponse
{
    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; } = new();

    [JsonPropertyName("info")]
    public Info Info { get; set; } = new();
}

public class Metadata
{
    [JsonPropertyName("matchId")]
    public string MatchId { get; set; } = string.Empty;

    [JsonPropertyName("participants")]
    public List<string> Participants { get; set; } = [];
}

public class Info
{
    [JsonPropertyName("gameDuration")]
    public int GameDuration { get; set; }

    [JsonPropertyName("participants")]
    public List<Participant> Participants { get; set; } = [];
}

public class Participant
{
    [JsonPropertyName("puuid")]
    public string Puuid { get; set; } = string.Empty;

    [JsonPropertyName("championName")]
    public string ChampionName { get; set; } = string.Empty;

    [JsonPropertyName("kills")]
    public int Kills { get; set; }

    [JsonPropertyName("deaths")]
    public int Deaths { get; set; }

    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    [JsonPropertyName("win")]
    public bool Win { get; set; }

    [JsonPropertyName("goldEarned")]
    public int GoldEarned { get; set; }

    [JsonPropertyName("totalDamageDealtToChampions")]
    public int Damage { get; set; }

    [JsonPropertyName("totalMinionsKilled")]
    public int Cs { get; set; }
}