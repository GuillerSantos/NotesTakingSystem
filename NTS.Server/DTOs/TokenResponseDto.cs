namespace NTS.Server.DTOs
{
    public class TokenResponseDto
    {
        #region Properties

        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        #endregion Properties
    }
}