using AuthTest.Interfaces;
using AuthTest.Models.Requests;
using AuthTest.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICustomLogger _customLogger;

        public UserController(IUserService userService, ICustomLogger customLogger)
        {
            _userService = userService;
            _customLogger = customLogger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirst("userId")?.Value;

            try
            {
                var result = await _userService.GetAsync(userId);

                UserResponse response = new UserResponse
                {
                    Name = result.Name,
                    Surname = result.Surname,
                    Email = result.Email,
                    Phone = result.Phone,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _customLogger.Log(ex);

                if (ex.Message.Contains("not found"))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EditUserDataRequest request)
        {
            var userId = User.FindFirst("userId")?.Value;

            try
            {
                var result = await _userService.EditUserDataAsync(request, userId);

                UserResponse response = new UserResponse
                {
                    Name = result.Name,
                    Surname = result.Surname,
                    Email = result.Email,
                    Phone = result.Phone,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _customLogger.Log(ex);

                if (ex.Message.Contains("not found"))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
