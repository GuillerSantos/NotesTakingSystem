using System.ComponentModel.DataAnnotations;

namespace NTS.Server.Domain.DTOs
{
    public class UserSignUpDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? RecoveryEmail { get; set; }
    }
}
