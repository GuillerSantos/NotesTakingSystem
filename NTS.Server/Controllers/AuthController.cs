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
        #region Fields

        private readonly IAuthService authService;
        private readonly IEmailService emailService;
        private readonly ILogger<AuthController> logger;

        #endregion Fields

        #region Public Constructors

        public AuthController(IAuthService authService, IEmailService emailService, ILogger<AuthController> logger)
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        [Authorize]
        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetAllUsersAccounts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await authService.GetAllUsersAccounts(page, pageSize));
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> LoginUsersAsync(LoginDto request)
        {
            var loggedIn = await authService.LoginUsersAsync(request);
            return loggedIn is null ? BadRequest("Invalid Credentials") : Ok(loggedIn);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return await authService.LogoutAsync(userId) ? Ok(new { message = "Successfully Logged Out" }) : BadRequest("Logout Failed");
        }

        [HttpPost("register-defaultuser")]
        public async Task<ActionResult<ApplicationUsers>> RegisterUserAsync(RegisterDto request)
        {
            var registeredUser = await authService.RegisterUsersAsync(request, false);
            return registeredUser is null ? BadRequest("Email Already Exists") : Ok(registeredUser);
        }

        [HttpPost("register-admin")]
        public async Task<ActionResult<ApplicationUsers>> RegisterAdminAsync(RegisterDto request)
        {
            if (!(User.Identity?.IsAuthenticated ?? false) || User.FindFirstValue(ClaimTypes.Role) != "Admin")
            {
                return Unauthorized("Only Authenticated Admins Can Create Admin Accounts");
            }

            var registeredAdmin = await authService.RegisterUsersAsync(request, true);
            return registeredAdmin is null ? BadRequest("Email Already Exists") : Ok(registeredAdmin);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            string resetToken = Guid.NewGuid().ToString();
            bool emailSent = await emailService.SendPasswordResetToRecoveryEmailAsync(request.RecoveryEmail, resetToken);
            return emailSent
                ? Ok(new { message = "Password Reset Email Has Been Sent" })
                : BadRequest(new { message = "This Email Address Is Not Registered Or There Was An Error Sending The Email" });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var refreshedToken = await authService.RefreshTokensAsync(request);
            return refreshedToken?.AccessToken is null || refreshedToken.RefreshToken is null
                ? Unauthorized("Invalid Refresh Token")
                : Ok(refreshedToken);
        }

        [HttpDelete("remove-account/{userId}")]
        public async Task<IActionResult> RemoveAccountAsync(Guid userId, [FromQuery] Guid noteId)
        {
            var removedAccount = await authService.RemoveAccountAsync(userId, noteId);
            return removedAccount ? Ok("Successfully Removed Account") : NotFound("Account Not Found");
        }

        #endregion Public Methods
    }
}