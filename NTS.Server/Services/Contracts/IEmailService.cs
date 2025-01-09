namespace NTS.Server.Services.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetToRecoveryEmailAsync(string userEmail, string resetToken);
    }
}
