using AuthPracticing.Repositories;
using AuthPracticing.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalAuthentication = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalAuthenticating(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalAuthenticationCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalAuthenticationCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Content("~/");
            var model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalAuthentication = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error : {remoteError}");
                return View("Login", model);
            }
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                ModelState.AddModelError("", $"An error occurred");
                return View("Login", model);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey, false, true);

            if (signInResult.Succeeded)
                return LocalRedirect(returnUrl);

            var email = externalLoginInfo.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);

            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var username = email.Split('@')[0];
                    user = new IdentityUser()
                    {
                      UserName = (username.Length < 10 ? username : username.Substring(0,10)),
                      Email = email,
                        EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(user);
                }
                await _userManager.AddLoginAsync(user, externalLoginInfo);
                await _signInManager.SignInAsync(user,false);

            return LocalRedirect(returnUrl);
            }
            ViewBag.ErrorTitle = $"Email claim not received from {externalLoginInfo.LoginProvider}";
            ViewBag.ErrorMessage = $"Can't Retrieve any data from the provider";
            return View();
        }
       
    }
}
