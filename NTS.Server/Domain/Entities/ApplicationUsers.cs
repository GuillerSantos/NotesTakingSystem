namespace NTS.Server.Domain.Entities
{
    public class ApplicationUsers
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RecoveryEmail { get; set; }
        public DateTime? DateJoined { get; set; } = DateTime.UtcNow;
        public string  Role { get; set; }
    }
}
