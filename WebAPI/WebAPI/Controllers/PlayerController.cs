using Habr.Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("current/email")]
        [Authorize()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentPlayerEmail()
        {
            var claims = HttpContext.User.Claims;

            return Ok(claims.First(claim => claim.Type == ClaimTypes.Email).Value);
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUpPlayerAsync([FromBody] SignUpPlayerRequest request)
        {
            await _playerService.SignUpPlayerAsync(request);

            return Ok();
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignInPlayerAsync([FromBody] SignInPlayerRequest request)
        {
            return Ok(await _playerService.SignInPlayerAsync(request));
        }

        [HttpPost("refresh-access-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokensDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshAccessTokenAsync([FromBody] RefreshAccessTokenRequest request)
        {
            return Ok(await _playerService.RefreshAccessTokenAsync(request));
        }
    }
}