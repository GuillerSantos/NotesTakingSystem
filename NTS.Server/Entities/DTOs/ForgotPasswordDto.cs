using System.ComponentModel.DataAnnotations;

namespace NTS.Server.Entities.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string RecoveryEmail { get; set; } = string.Empty;
    }
}
