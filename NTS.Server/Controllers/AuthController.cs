using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.DTOs;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;
using System.Security.Claims;

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
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetAllUsersAccounts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var fetchedAccounts = await authService.GetAllUsersAccounts(page, pageSize);
                return Ok(fetchedAccounts);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Internal Server Error: {error.Message}");
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> LoginUsersAsync(LoginDto request)
        {
            try
            {
                var loggedIn = await authService.LoginUsersAsync(request);

                if (loggedIn is null)
                {
                    return BadRequest("Invalid Credentials");
                }

                return Ok(loggedIn);
            }
            catch (Exception error)
            {
                return BadRequest(new { message = "An Error Occurred During Login", details = error.Message });
            }
        }


        [HttpPost("logout"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await authService.LogoutAsync(userId);

                if (result)
                {
                    return Ok(new { message = "Successfully Logged Out"});
                }

                return BadRequest("Logout Failed");
            }
            catch(Exception error)
            {
                return StatusCode(500, $"Internal Server Error: {error.Message}");
            }
        }


        [HttpPost("register-defaultuser")]
        public async Task<ActionResult<ApplicationUsers>> RegisterUserAsync(RegisterDto request)
        {
            try
            {
                var registeredUser = await authService.RegisterUsersAsync(request, false);

                if (registeredUser is null)
                {
                    return BadRequest("Email Already Exists");
                }

                return Ok(registeredUser);
            }
            catch (Exception error)
            {
                return BadRequest(new { message = "An Error Occurred During Registration", details = error.Message });
            }
        }


        [HttpPost("register-admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationUsers>> RegisterAdminAsync(RegisterDto request)
        {
            try
            {
                var isAuthenticated = User!.Identity?.IsAuthenticated ?? false;
                var currentUserRole = User!.FindFirstValue(ClaimTypes.Role);

                if (!isAuthenticated || currentUserRole != "Admin")
                {
                    return Unauthorized("Only Authenticated Admins Can Create Admin Accounts");
                }

                var registeredAdmin = await authService.RegisterUsersAsync(request, true);

                if (registeredAdmin is null)
                {
                    return BadRequest("Email Already Exists");
                }

                return Ok(registeredAdmin);
            }
            catch (Exception error)
            {
                return BadRequest(new { message = "An Error Occurred During Registration", details = error.Message });
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            try
            {
                string resetToken = Guid.NewGuid().ToString();

                bool emailSent = await emailService.SendPasswordResetToRecoveryEmailAsync(request.RecoveryEmail, resetToken);

                if (!emailSent)
                {
                    return BadRequest(new { message = "This Email Address Is Not Registered Or There Was An Error Sending The Email" });
                }

                return Ok(new { message = "Password Reset Email Has Been Sent" });
            }
            catch (Exception error)
            {
                logger.LogError($"An Error Occurred While Handling Forgot Password Request: {error.Message}");
                return BadRequest(new { message = "An Error Occurred During Forgot Password", details = error.Message });
            }
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            try
            {
                var resfreshedToken = await authService.RefreshTokensAsync(request);
                if (resfreshedToken is null || resfreshedToken.AccessToken is null || resfreshedToken.RefreshToken is null)
                {
                    return Unauthorized("Invalid Refresh Token");
                }

                return Ok(resfreshedToken);
            }
            catch (Exception error)
            {
                return BadRequest($"Error Refreshing The Token: {error.Message}");
            }
        }


        [HttpDelete("remove-account/{userId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAccountAsync(Guid userId, Guid noteId)
        {
            try
            {
                var removedAccount = await authService.RemoveAccountAsync(userId, noteId);

                if (!removedAccount)
                {
                    return NotFound("Account Not Found");
                }
                else
                {
                    return Ok("Successfully Removed Account");
                }
            }
            catch (Exception error)
            {
                return BadRequest($"Error Removing Account: {error.Message}");
            }
        }
    }
}
