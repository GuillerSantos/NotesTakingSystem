using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IEmailService emailService;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthService authService, IEmailService emailService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.emailService = emailService;
            this.logger = logger;
        }


        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> LoginUsersAsync(LoginDto request)
        {
            try
            {
                var result = await authService.LoginUsersAsync(request);

                if (result is null)
                {
                    return BadRequest("Invalid Credentials");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An Error Occurred During Login", details = ex.Message });
            }
        }


        [HttpPost("register"), Authorize (Roles = "User")]
        public async Task<ActionResult<ApplicationUsers>> RegisterUserAsync(SignUpDto request)
        {
            try
            {
                var user = await authService.RegisterUsersAsync(request);

                if (user is null)
                    return BadRequest("Email Already Exists");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An Error Occurred During Registration", details = ex.Message });
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            try
            {
                string resetToken = Guid.NewGuid().ToString();

                bool emailSent = await emailService.SendPasswordResetToRecoveryEmailAsync(request.Email, resetToken);

                if (!emailSent)
                {
                    return BadRequest(new { message = "This Email Address Is Not Registered Or There Was An Error Sending The Email" });
                }

                return Ok(new { message = "Password Reset Email Has Been Sent" });
            }
            catch (Exception ex)
            {
                logger.LogError($"An Error Occurred While Handling Forgot Password Request: {ex.Message}", ex);
                return BadRequest(new { message = "An Error Occurred During Forgot Password", details = ex.Message });
            }
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid Refresh Token");
            }

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetAllUsersAccounts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var usersAccounts = await authService.GetAllUsersAccounts(page, pageSize);
                return Ok(usersAccounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
