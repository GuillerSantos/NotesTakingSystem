using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly ApplicationDbContext dbContext;
        private readonly EmailSettingsDto emailSettings;
        private readonly ILogger<EmailService> logger;

        public EmailService(ApplicationDbContext dbContext, IOptions<EmailSettingsDto> emailSettings, ILogger<EmailService> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<bool> SendPasswordResetToRecoveryEmailAsync(string userRecoveryEmail, string resetToken)
        {
            var user = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.RecoveryEmail == userRecoveryEmail);

            if (user == null)
            {
                logger.LogWarning($"No User Found With This Recovery Email {userRecoveryEmail}");
                return false;
            }

            var fromEmail = emailSettings.FromEmail;
            var senderPassword = emailSettings.AppPassword;

            var toEmail = userRecoveryEmail;
            var subject = "Password Reset Request";
            var body = $"Please Click On The Following Link To Reset Your Password: https://notestakingsystem.com/reset-password?token={resetToken}";

            var smtpClient = new SmtpClient(emailSettings.SMTPServer)
            {
                Port = emailSettings.SMTPPort,
                Credentials = new NetworkCredential(fromEmail, senderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, emailSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                logger.LogInformation($"Password Reset Email Sent Successfully To: {toEmail}");
                return true;
            }
            catch (Exception error)
            {
                logger.LogError($"Error Sending Password Reset Email To: {toEmail}: {error.Message}");
                return false;
            }
        }
    }
}
