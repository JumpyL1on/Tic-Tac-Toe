using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Interfaces;
using WebAPI.Requests;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    [Authorize()]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly UserManager<Player> _userManager;

        public GameController(IGameService gameService, UserManager<Player> userManager)
        {
            _gameService = gameService;
            _userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateGameAsync()
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            await _gameService.CreateGameAsync(player.Id);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CloseGameAsync([FromRoute] int id)
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            await _gameService.CloseGameAsync(id, player.Id);

            return Ok();
        }

        [HttpPatch("{id:int}/playerBId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddAnotherPlayerToTheGameAsync([FromRoute] int id)
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            await _gameService.AddAnotherPlayerToTheGameAsync(id, player.Id);

            return Ok();
        }

        [HttpPatch("{id:int}/field")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MakeAPlayerMoveAsync([FromRoute] int id, [FromBody] MakeAPlayerMoveRequest request)
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            await _gameService.MakeAPlayerMoveAsync(id, player.Id, request);

            return Ok();
        }
    }
}