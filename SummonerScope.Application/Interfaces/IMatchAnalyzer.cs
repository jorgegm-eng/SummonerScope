using SummonerScope.Application.DTOs;

namespace SummonerScope.Application.Interfaces;

public interface IMatchAnalyzer
{
    MatchAnalysisDto AnalyzeMatch(MatchParticipantStatsDto stats);
    PlayerMatchAnalysisDto AnalyzePlayerMatch(MatchParticipantStatsDto stats);
}