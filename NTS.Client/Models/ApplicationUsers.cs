using System.ComponentModel.DataAnnotations;

namespace NTS.Client.Models
{
    public class ApplicationUsers
    {
        #region Properties

        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RecoveryEmail { get; set; } = string.Empty;
        public DateTime? DateJoined { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }

        #endregion Properties
    }
}