namespace NTS.Server.DTOs
{
    public class UsersDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? DateJoined { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
