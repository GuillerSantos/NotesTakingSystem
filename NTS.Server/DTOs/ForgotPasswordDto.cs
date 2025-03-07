using System.ComponentModel.DataAnnotations;

namespace NTS.Server.DTOs
{
    public class ForgotPasswordDto
    {
        #region Properties

        [Required]
        [EmailAddress]
        public string RecoveryEmail { get; set; } = string.Empty;

        #endregion Properties
    }
}