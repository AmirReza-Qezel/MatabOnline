using System.ComponentModel.DataAnnotations;

namespace Application.AdminUserAgg
{
    public record AdminUserDto(
        string Id, string Email, string FirstName, string LastName, string NCode);

    public record UserClaimDto(string ClaimType, string ClaimValue, bool IsGranted);

    public class CreateAdminUserDto
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public string NCode { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    }

    // The full set of permissions the admin.html mockup lets you toggle per user.
    public static class Permissions
    {
        public const string ClaimType = "Permission";

        public const string ViewDoctor = "ViewDoctor";
        public const string EditDoctor = "EditDoctor";
        public const string ManageDoctors = "ManageDoctors";

        public static readonly string[] All = { ViewDoctor, EditDoctor, ManageDoctors };
    }
}
