using apartease_backend.Dao;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace apartease_backend.Services.EmailService
{
    public class EmailServiceImpl: IEmailService
    {

        private readonly EmailConfiguration _emailSettings;

        public EmailServiceImpl(IOptions<EmailConfiguration> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }


        public async Task SendAppEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSsl);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmailAsync(string fromEmail, string toEmail, string fromName, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(fromName, fromEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSsl);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
