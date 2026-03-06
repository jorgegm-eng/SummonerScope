using SummonerScope.Application.DTOs;

namespace SummonerScope.Application.Interfaces;

public interface IPlayerService
{
    Task<PlayerDto?> GetPlayerAsync(string region, string gameName, string tagLine);
    Task<PlayerMatchesDto?> GetPlayerMatchesAsync(string region, string gameName, string tagLine, int count = 10);
    Task<MatchAnalysisDto?> GetMatchAnalysisAsync(string matchId, string puuid);
    Task<List<PlayerMatchAnalysisDto>?> GetPlayerMatchAnalysisAsync(string region, string gameName, string tagLine, int count = 10);
}