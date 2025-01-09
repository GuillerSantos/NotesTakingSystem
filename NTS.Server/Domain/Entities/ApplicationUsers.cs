namespace NTS.Server.Domain.Entities
{
    public class ApplicationUsers
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? RecoveryEmail { get; set; }
        public DateTime? DateJoined { get; set; } = DateTime.UtcNow;
        public string  Role { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
