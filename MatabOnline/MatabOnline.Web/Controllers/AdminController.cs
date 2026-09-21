using Application.AdminUserAgg;
using Application.DoctorAgg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize] // must be a logged-in admin user at minimum for the whole controller
    public class AdminController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IUserManagementService _userManagementService;

        public AdminController(IDoctorService doctorService, IUserManagementService userManagementService)
        {
            _doctorService = doctorService;
            _userManagementService = userManagementService;
        }

        // Matches admin.html: doctor list, user list with claims, create-user form.
        [Authorize(Policy = Permissions.ViewDoctor)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            ViewBag.Users = await _userManagementService.GetAllUsersAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ManageDoctors)] // only users with ManageDoctors can create admin accounts
        public async Task<IActionResult> CreateUser(CreateAdminUserDto dto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var (succeeded, errors) = await _userManagementService.CreateUserAsync(dto);

            TempData[succeeded ? "UserSuccess" : "UserError"] = succeeded
                ? "کاربر با موفقیت ایجاد شد"
                : string.Join(" | ", errors);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ManageDoctors)]
        public async Task<IActionResult> UpdateUserClaims(string userId, List<string> grantedPermissions)
        {
            await _userManagementService.SetUserPermissionsAsync(userId, grantedPermissions ?? new());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.EditDoctor)]
        public async Task<IActionResult> CreateDoctor(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["DoctorError"] = "اطلاعات وارد شده معتبر نیست";
                return RedirectToAction(nameof(Index));
            }

            await _doctorService.CreateAsync(dto);
            TempData["DoctorSuccess"] = "پزشک با موفقیت ایجاد شد";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.EditDoctor)]
        public async Task<IActionResult> EditDoctor(UpdateDoctorDto dto)
        {
            if (ModelState.IsValid)
                await _doctorService.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.ManageDoctors)]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _doctorService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
