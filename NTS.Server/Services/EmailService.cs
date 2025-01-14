using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTS.Server.Database.DatabaseContext;
using NTS.Server.Entities.DTOs;
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
            this.dbContext = dbContext;
            this.emailSettings = emailSettings.Value;
            this.logger = logger;
        }

        public async Task<bool> SendPasswordResetToRecoveryEmailAsync(string userEmail, string resetToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.RecoveryEmail == userEmail);

            if (user == null)
            {
                logger.LogWarning($"No User Found With This Recovery Email {userEmail}");
                return false;
            }

            var fromEmail = emailSettings.FromEmail;
            var senderPassword = emailSettings.AppPassword;

            var toEmail = userEmail;
            var subject = "Password Reset Request";
            var body = $"Please Click on the Following Link to Reset your Password: https://notestakingsystem.com/reset-password?token={resetToken}";

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
            catch (Exception ex)
            {
                logger.LogError($"Error Sending Password Reset Email To: {toEmail}: {ex.Message}", ex);
                return false;
            }
        }
    }
}
