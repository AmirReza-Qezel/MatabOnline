using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.AdminUserAgg
{
    public class AdminUser : IdentityUser
    {
        public AdminUser() { }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NCode { get; set; } = string.Empty;

    }
}
