namespace NTS.Client.Models.DTOs
{
    public class ResponseTokenDto
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string TokenResponse { get; set; } = string.Empty;
        public string ResetToken { get; set; } = string.Empty;
    }
}
