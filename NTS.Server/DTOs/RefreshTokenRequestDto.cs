namespace NTS.Server.DTOs
{
    public class RefreshTokenRequestDto
    {
        #region Properties

        public Guid UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;

        #endregion Properties
    }
}