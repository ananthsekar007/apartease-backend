namespace apartease_backend.Services.EmailService
{
    public interface IEmailService
    {
        Task SendAppEmailAsync(string email, string subject, string message);
        Task SendEmailAsync(string fromEmail, string toEmail, string fromName, string subject, string message);
    }
}
