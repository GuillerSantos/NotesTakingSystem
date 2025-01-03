using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Domain.DTOs;
using NTS.Server.Services;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Controller
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpGet("get-all-users"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UsersDto>>> GetAllUsersAccounts()
        {
            try
            {
                var accounts = await authService.GetAllUsersAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPost("register-user"), Authorize (Roles = "User")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] SignUpDto request)
        {
            try
            {
                var user = await authService.RegisterUsersAsync(request, "User");

                return Ok(new
                {
                    message = "User Registered Successfully",
                    userId = user.UserId,
                    fullName = user.FullName,
                    email = user.Email,
                    role = user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An Error Occurred During Registration", details = ex.Message });
            }
        }

        [HttpPost("register-admin"), Authorize (Roles = "Admin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] SignUpDto request)
        {
            try
            {
                var user = await authService.RegisterUsersAsync(request, "Admin");

                return Ok(new
                {
                    message = "Admin Registered Successfully",
                    userId = user.UserId,
                    fullName = user.FullName,
                    email = user.Email,
                    role = user.Role
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
                return BadRequest(new { message = "An Error Occured During Login", details = ex.Message });
            }
        }
    }
}