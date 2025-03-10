using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTS.Server.Data;
using NTS.Server.DTOs;
using NTS.Server.Services.Contracts;
using System.Net;
using System.Net.Mail;

namespace NTS.Server.Services
{
    public class EmailService : IEmailService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;
        private readonly EmailSettingsDto emailSettings;
        private readonly ILogger<EmailService> logger;

        #endregion Fields

        #region Public Constructors

        public EmailService(ApplicationDbContext dbContext, IOptions<EmailSettingsDto> emailSettings, ILogger<EmailService> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> SendPasswordResetToRecoveryEmailAsync(string userRecoveryEmail, string resetToken)
        {
            var user = await dbContext.ApplicationUsers
                .FirstOrDefaultAsync(u => u.RecoveryEmail == userRecoveryEmail);

            if (user == null)
            {
                logger.LogWarning($"No user found with recovery email: {userRecoveryEmail}");
                return false;
            }

            var mailMessage = CreateMailMessage(userRecoveryEmail, resetToken);
            return await SendEmailAsync(mailMessage);
        }

        #endregion Public Methods

        #region Private Methods

        private MailMessage CreateMailMessage(string toEmail, string resetToken)
        {
            var resetLink = $"https://notestakingsystem.com/reset-password?token={resetToken}";
            var body = $"Please click on the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";

            return new MailMessage
            {
                From = new MailAddress(emailSettings.FromEmail, emailSettings.FromName),
                Subject = "Password Reset Request",
                Body = body,
                IsBodyHtml = true,
                To = { toEmail }
            };
        }

        private async Task<bool> SendEmailAsync(MailMessage mailMessage)
        {
            using var smtpClient = new SmtpClient(emailSettings.SMTPServer)
            {
                Port = emailSettings.SMTPPort,
                Credentials = new NetworkCredential(emailSettings.FromEmail, emailSettings.AppPassword),
                EnableSsl = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                logger.LogInformation($"Password reset email sent successfully to: {mailMessage.To}");
                return true;
            }
            catch (SmtpException smtpEx)
            {
                logger.LogError($"SMTP error sending password reset email to: {mailMessage.To} - {smtpEx.Message}");
            }
            catch (InvalidOperationException invEx)
            {
                logger.LogError($"Invalid operation while sending email to: {mailMessage.To} - {invEx.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected error sending email to: {mailMessage.To} - {ex.Message}");
            }

            return false;
        }

        #endregion Private Methods
    }
}