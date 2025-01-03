namespace NTS.Server.Domain.DTOs
{
    public class UsersDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DateJoined { get; set; }
        public string Role { get; set; }
    }
}
