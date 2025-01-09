namespace NTS.Server.Domain.DTOs
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }= string.Empty;
    }
}
