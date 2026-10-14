
using System.Net.Mail;
using System.Net;

namespace AuthPracticing.Repositories
{
    public class MessageSender : IMessageSender
    {
        public Task EmailSend(string toMail, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    Domain = "",
                    Password = ""
                };
                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Port = 587;
                client.Host = "localhost";
                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toMail) },
                    From = new MailAddress(""),
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
