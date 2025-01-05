namespace NTS.Server.Services.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetEmailAsync(string userEmail, string resetToken);
    }
}
