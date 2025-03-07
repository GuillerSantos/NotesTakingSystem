namespace NTS.Server.DTOs
{
    public class LoginResponseDto
    {
        #region Properties

        public string Token { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        #endregion Properties
    }
}