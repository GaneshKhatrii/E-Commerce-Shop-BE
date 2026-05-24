using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = result,
                StatusCode = 200,
                Data = null
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            var response = new ApiResponse<LoginResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Login successful",
                Data = result
            };

            return Ok(response);
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            await _authService.VerifyEmailAsync(token);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "Email verified successfully",
                Data = null
            };

            return Ok(response);
        }
    }
}
