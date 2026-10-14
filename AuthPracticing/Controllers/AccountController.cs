using AuthPracticing.Repositories;
using AuthPracticing.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthPracticing.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageSender _messageSender;

        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IMessageSender messageSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageSender = messageSender;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        { 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    var emailConfirmationToken =
                        await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailMessage =
                        Url.Action("ConfirmEmail","Account",
                        new { username = user.UserName, token=emailConfirmationToken },
                        Request.Scheme);
                    await _messageSender.EmailSend(model.Email, "Email Confirmation", emailMessage);
                    return RedirectToAction("Index","Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        private async Task<IActionResult> ConfirmationEmail(string username, string token)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");
            
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl =null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName)
                   ?? await _userManager.FindByEmailAsync(model.UserName);

                if (user == null) { ModelState.AddModelError("", "کاربر یافت نشد"); return View(model); }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ViewBag.Error = "Too many attempts, you can't try to log in for five minutes";
                    return View(model);
                }
                ModelState.AddModelError("", "Incorrect username or password");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUsernameAvailable(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) { return Json(true); }
            return Json("Username already exists");
        }

    }
}
