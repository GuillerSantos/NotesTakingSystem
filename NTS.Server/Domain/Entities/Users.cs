namespace NTS.Server.Domain.Entities
{
    public class Users
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RecoveryEmail { get; set; }
        public DateTime? DateJoined { get; set; } = DateTime.UtcNow;
    }
}
