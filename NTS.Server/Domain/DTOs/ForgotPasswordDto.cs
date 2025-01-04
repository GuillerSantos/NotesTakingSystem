using System.ComponentModel.DataAnnotations;

namespace NTS.Server.Domain.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
