using ArticlesManagement.Application.Interfaces;
using ArticlesManagement.Application.Models.Requests;
using ArticlesManagement.Application.Models.Responses;
using ArticlesManagement.Application.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticlesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Register logic
            var result = await _authService.Register(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Login logic
            var result = await _authService.Login(request);
            if (result.Token != null)
            {
                return Ok(result);
            }
            return Unauthorized(result.Errors);
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            int userId = int.Parse(userIdClaim.Value);

            var user = await _authService.GetUserById(userId);

            if (user == null)
            {
                return NotFound(); // Return not found if user does not exist
            }

            var result = new BaseResult<GetUserResponse> { Result = user };


            if (!user.IsVerified)
            {
                result.Errors = new List<string> { "User is not verified" };

                return BadRequest(result);
            };

            return Ok(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var result = await _authService.VerifyEmail(request);

            return Ok(result);
        }

    }
}
