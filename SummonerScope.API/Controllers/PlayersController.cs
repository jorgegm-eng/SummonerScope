using Microsoft.AspNetCore.Mvc;
using SummonerScope.Application.Interfaces;

namespace SummonerScope.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet("{region}/{gameName}/{tagLine}")]
    public async Task<IActionResult> GetPlayer(string region, string gameName, string tagLine)
    {
        var player = await _playerService.GetPlayerAsync(region, gameName, tagLine);

        if (player is null)
        {
            return NotFound();
        }

        return Ok(player);
    }

    [HttpGet("{region}/{gameName}/{tagLine}/matches")]
    public async Task<IActionResult> GetPlayerMatches(string region, string gameName, string tagLine, [FromQuery] int count = 10)
    {
        var matches = await _playerService.GetPlayerMatchesAsync(region, gameName, tagLine, count);

        if (matches is null)
        {
            return NotFound();
        }

        return Ok(matches);
    }
}