using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ContactMessageAgg
{
    public class ContactMessage : BaseEntity
    {
        public ContactMessage(string fullName, string email, string message, bool isRead)
        {
            FullName = fullName;
            Email = email;
            Message = message;
            IsRead = isRead;
        }

        protected ContactMessage() { }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public void Edit(string fullName, string email, string message, bool isRead)
        {
            FullName = fullName;
            Email = email;
            Message = message;
            IsRead = isRead;
        }
    }
}
