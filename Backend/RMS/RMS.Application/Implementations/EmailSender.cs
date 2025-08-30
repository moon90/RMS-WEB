using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using RMS.Application.Interfaces;
namespace RMS.Application.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = body
                };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), true);
                await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the exception internally, e.g., to a file or a monitoring system.
                // For this exercise, we'll just print to console.
                Console.WriteLine($"Error sending email: {ex.Message}");
                // Optionally re-throw if the calling service needs to handle it, or return a custom error object.
                // throw;
            }
        }
    }
}
