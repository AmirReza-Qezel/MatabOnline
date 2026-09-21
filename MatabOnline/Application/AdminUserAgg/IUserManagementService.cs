namespace Application.AdminUserAgg
{
    public interface IUserManagementService
    {
        Task<IReadOnlyList<AdminUserDto>> GetAllUsersAsync();

        // Returns every permission in Permissions.All, flagged with whether this user has it -
        // exactly what the admin.html checkbox grid needs to render.
        Task<IReadOnlyList<UserClaimDto>> GetUserClaimsAsync(string userId);

        Task<(bool Succeeded, IEnumerable<string> Errors)> CreateUserAsync(CreateAdminUserDto dto);

        // Reconciles the user's claims to exactly match grantedPermissions
        // (adds missing ones, removes ones no longer checked).
        Task SetUserPermissionsAsync(string userId, IReadOnlyCollection<string> grantedPermissions);
    }
}
