using System.ComponentModel.DataAnnotations;

namespace NTS.Server.DTOs
{
    public class LoginDto
    {
        #region Properties

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        #endregion Properties
    }
}