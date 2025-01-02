namespace NTS.Server.Domain.DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
