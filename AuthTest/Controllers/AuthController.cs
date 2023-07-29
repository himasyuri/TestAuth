using AuthTest.Interfaces;
using AuthTest.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IJwtService _jwtService;
        private readonly ICustomLogger _customLogger;

        public AuthController(IUserAuthService userAuthService,
            ICustomLogger customLogger, IJwtService jwtService)
        {
            _userAuthService = userAuthService;
            _customLogger = customLogger;
            _jwtService = jwtService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _userAuthService.RegisterAsync(request);
                var tokens = await _jwtService.CreateTokensAsync(user);

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                _customLogger.Log(ex);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userAuthService.LoginAsync(request);
                var tokens = await _jwtService.CreateTokensAsync(user);

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                _customLogger.Log(ex);

                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            try
            {
                var tokens = await _jwtService.RefreshAsync(refreshToken);

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                _customLogger.Log(ex);

                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string token)
        {
            try
            {
                await _jwtService.RevokeAsync(token);

                return Ok();
            }
            catch (Exception ex)
            {
                _customLogger.Log(ex);

                return BadRequest(ex);
            }
        }
    }
}
