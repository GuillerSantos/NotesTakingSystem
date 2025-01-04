namespace NTS.Server.Services.Contracts
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string userEmail, string resetToken);
    }
}
