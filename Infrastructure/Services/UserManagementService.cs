using Application.AdminUserAgg;
using Domain.AdminUserAgg;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services
{
    // Implementation lives in Infrastructure (not Application) because it depends
    // on UserManager<T> - a concrete ASP.NET Core Identity type. Application only
    // ever sees IUserManagementService.
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<AdminUser> _userManager;

        public UserManagementService(UserManager<AdminUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<AdminUserDto>> GetAllUsersAsync()
        {
            var users = await Task.FromResult(_userManager.Users.ToList());
            return users.Select(u => new AdminUserDto(
                u.Id, u.Email ?? string.Empty, u.FirstName, u.LastName, u.NCode)).ToList();
        }

        public async Task<IReadOnlyList<UserClaimDto>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new KeyNotFoundException("کاربر یافت نشد");

            var existingClaims = await _userManager.GetClaimsAsync(user);
            var grantedValues = existingClaims
                .Where(c => c.Type == Permissions.ClaimType)
                .Select(c => c.Value)
                .ToHashSet();

            return Permissions.All
                .Select(p => new UserClaimDto(Permissions.ClaimType, p, grantedValues.Contains(p)))
                .ToList();
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(CreateAdminUserDto dto)
        {
            var user = new AdminUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NCode = dto.NCode
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        public async Task SetUserPermissionsAsync(string userId, IReadOnlyCollection<string> grantedPermissions)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new KeyNotFoundException("کاربر یافت نشد");

            var currentClaims = (await _userManager.GetClaimsAsync(user))
                .Where(c => c.Type == Permissions.ClaimType)
                .ToList();

            // Reconcile: remove anything unchecked, add anything newly checked.
            var toRemove = currentClaims.Where(c => !grantedPermissions.Contains(c.Value));
            var toAdd = grantedPermissions
                .Where(p => !currentClaims.Any(c => c.Value == p))
                .Select(p => new Claim(Permissions.ClaimType, p));

            if (toRemove.Any())
                await _userManager.RemoveClaimsAsync(user, toRemove);

            if (toAdd.Any())
                await _userManager.AddClaimsAsync(user, toAdd);
        }
    }
}
