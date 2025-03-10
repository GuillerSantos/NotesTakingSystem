﻿namespace NTS.Client.DTOs
{
    public class ResponseTokenDto
    {
        #region Properties

        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string TokenResponse { get; set; } = string.Empty;
        public string ResetToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        #endregion Properties
    }
}