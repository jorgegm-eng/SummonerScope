using Microsoft.AspNetCore.Mvc;
using SummonerScope.Application.Interfaces;

namespace SummonerScope.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public MatchesController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet("{matchId}/analysis")]
    public async Task<IActionResult> GetMatchAnalysis(string matchId, [FromQuery] string puuid)
    {
        if (string.IsNullOrWhiteSpace(puuid))
        {
            return BadRequest("The puuid query parameter is required.");
        }

        var analysis = await _playerService.GetMatchAnalysisAsync(matchId, puuid);

        if (analysis is null)
        {
            return NotFound();
        }

        return Ok(analysis);
    }
}