using System.Net;
using System.Net.Mail;

namespace AuthPracticing.Repositories
{
    public interface IMessageSender
    {
        public Task EmailSend(string toMail, string subject, string message, bool isMessageHtml = false);
    }
}
