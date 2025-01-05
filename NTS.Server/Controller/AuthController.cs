using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NTS.Server.Domain.DTOs;
using NTS.Server.Services;
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


        [HttpGet("get-all-users"), Authorize(Roles = "Admin")]
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


        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpDto request)
        {
            try
            {
                var user = await authService.RegisterUsersAsync(request, "User");

                return Ok(new
                {
                    message = "User Registered Successfully",
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An Error Occurred During Registration", details = ex.Message });
            }
        }


        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] SignUpDto request)
        {
            try
            {
                var user = await authService.RegisterUsersAsync(request, "Admin");

                return Ok(new
                {
                    message = "Admin Registered Successfully",
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.Role
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = "An Error Occurred During Registration", details = ex.Message });
            }
        }


        [HttpPost("login-users")]
        public async Task<IActionResult> LoginUsersAsync([FromBody] LoginDto request)
        {
            try
            {
                var token = await authService.LoginUsersAsync(request);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An Error Occurred During Login", details = ex.Message });
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            try
            {
                string resetToken = Guid.NewGuid().ToString();

                bool emailSent = await emailService.SendPasswordResetEmailAsync(request.Email, resetToken);

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
    }
}