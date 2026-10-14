using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthPracticing.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required, Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required, Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "UserName")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public List<AuthenticationScheme> ExternalAuthentication { get; set; }
    }
}
