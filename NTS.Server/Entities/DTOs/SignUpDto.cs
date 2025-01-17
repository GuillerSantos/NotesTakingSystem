using System.ComponentModel.DataAnnotations;

namespace NTS.Server.Entities.DTOs
{
    public class SignUpDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string RecoveryEmail { get; set; } = string.Empty;

        [Required]
        public DateTime? DateJoined { get; set; } = DateTime.UtcNow;
    }
}
