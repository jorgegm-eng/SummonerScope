using SummonerScope.Application.DTOs;
using SummonerScope.Infrastructure.RiotAPI.Models;

namespace SummonerScope.Application.Interfaces;

public interface IMatchAnalyzer
{
    MatchAnalysisDto? AnalyzeMatch(RiotMatchResponse match, string puuid);
    PlayerMatchAnalysisDto? AnalyzePlayerMatch(RiotMatchResponse match, string puuid);
}