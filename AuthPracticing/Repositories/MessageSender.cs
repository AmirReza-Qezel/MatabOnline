using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration; // Add this

namespace AuthPracticing.Repositories
{
    public class MessageSender : IMessageSender
    {
        private readonly IConfiguration _config;

        // Inject configuration via constructor
        public MessageSender(IConfiguration config)
        {
            _config = config;
        }

        public Task EmailSend(string toMail, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    // Read from configuration instead of hardcoding
                    UserName = _config["EmailSettings:Username"],
                    Password = _config["EmailSettings:Password"]
                };

                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Port = int.Parse(_config["EmailSettings:Port"] ?? "587");
                client.Host = _config["EmailSettings:Host"];

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toMail) },
                    From = new MailAddress(_config["EmailSettings:SenderEmail"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };

                client.Send(emailMessage);
                return Task.CompletedTask;
            }
        }
    }
}