using Microsoft.Extensions.Options;
using ParsFile.Application.Contracts.Services;
using ParsFile.Domain.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ParsFile.Infrastructure.Services.Mail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task Execute(string email, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 1000000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password);

            MailMessage message = new MailMessage(_emailSettings.From, email, subject, body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            client.Send(message);
            return Task.CompletedTask;
        }
    }
}
