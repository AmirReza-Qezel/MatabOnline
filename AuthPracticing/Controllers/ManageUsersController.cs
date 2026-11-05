using AuthPracticing.Repositories;
using AuthPracticing.ViewModels.ManageUser;
using IdentitySample.ViewModels.ManageUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthPracticing.Controllers
{
    public class ManageUsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ManageUsersController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var model = _userManager.Users.
                Select(x => new IndexViewModel()
                {Id = x.Id, UserName = x.UserName, Email = x.Email }).ToList();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByNameAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id,string userName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(userName)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.UserName = userName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = _roleManager.Roles.AsNoTracking().Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var validRoles = roles.Except(userRoles)
                .Select(r => new UserRolesViewModel() { RoleName = r }).ToList();

            var model = new AddUserToRoleViewModel() { UserId = id, UserRoles = validRoles };

            //var model = new AddUserToRoleViewModel() { UserId = id };

            //foreach (var role in roles)
            //{
            //    if (!await _userManager.IsInRoleAsync(user, role.Name))
            //    {
            //        model.UserRoles.Add(new UserRolesViewModel()
            //        {
            //            RoleName = role.Name
            //        });
            //    }
            //}

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestRoles = model.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManager.AddToRolesAsync(user, requestRoles);

            if (result.Succeeded) return RedirectToAction("index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            var validRoles = roles.Select(r => new UserRolesViewModel() { RoleName = r }).ToList();

            var model = new AddUserToRoleViewModel() { UserId = id, UserRoles = validRoles };
            //var model = new AddUserToRoleViewModel() { UserId = id };

            //foreach (var role in roles)
            //{
            //    if (await _userManager.IsInRoleAsync(user, role.Name))
            //    {
            //        model.UserRoles.Add(new UserRolesViewModel()
            //        {
            //            RoleName = role.Name
            //        });
            //    }
            //}

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(AddUserToRoleViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestRoles = model.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManager.RemoveFromRolesAsync(user, requestRoles);

            if (result.Succeeded) return RedirectToAction("index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToClaim(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var allClaims = ClaimStore.AllClaims;
            if (!allClaims.Any()) return NotFound();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var validClaims = allClaims.Except(userClaims)
                .Select(c => new ClaimsViewModel() { ClaimType = c.Type}).ToList();
            var model = new AddOrRemoveClaimViewModel() { UserId = id, UserClaims = validClaims };
            return View(model);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToClaim(AddOrRemoveClaimViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user == null) return NotFound();
            var requestClaims = model.UserClaims.Where(r => r.IsSelected)
                .Select(u => new Claim(u.ClaimType, true.ToString())).ToList();
            var result = await _userManager.AddClaimsAsync(user, requestClaims);
            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromClaim(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var allClaims = ClaimStore.AllClaims;
            if (!allClaims.Any()) return NotFound();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var validClaims = userClaims
                .Select(c => new ClaimsViewModel() { ClaimType = c.Type }).ToList();
            var model = new AddOrRemoveClaimViewModel() { UserId = id, UserClaims = validClaims };
            return View(model);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromClaim(AddOrRemoveClaimViewModel model)
        {
            if (model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestClaims = model.UserClaims.Where(r => r.IsSelected)
                .Select(u => new Claim(u.ClaimType, true.ToString())).ToList();
                 var result = await _userManager.RemoveClaimsAsync(user, requestClaims);

            if (result.Succeeded) return RedirectToAction("Index");
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    }
}
