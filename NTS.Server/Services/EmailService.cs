using Microsoft.Extensions.Options;
using NTS.Server.Domain.Models;
using NTS.Server.Services.Contracts;
using System.Net;
using System.Net.Mail;

namespace NTS.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public async Task SendPasswordResetEmailAsync(string userEmail, string resetToken)
        {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Sending Email: {ex.Message}");
                throw;
            }
        }
    }
}
