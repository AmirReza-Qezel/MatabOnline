using Application.AdminUserAgg;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents
{
    // Renders the checkbox grid (ViewDoctor / EditDoctor / ManageDoctors) for one user.
    public class UserClaimsViewComponent : ViewComponent
    {
        private readonly IUserManagementService _userManagementService;

        public UserClaimsViewComponent(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var claims = await _userManagementService.GetUserClaimsAsync(userId);
            return View((userId, claims));
        }
    }
}
