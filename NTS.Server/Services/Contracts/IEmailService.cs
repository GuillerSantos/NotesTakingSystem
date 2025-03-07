namespace NTS.Server.Services.Contracts
{
    public interface IEmailService
    {
        #region Public Methods

        Task<bool> SendPasswordResetToRecoveryEmailAsync(string userEmail, string resetToken);

        #endregion Public Methods
    }
}