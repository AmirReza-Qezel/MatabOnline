using Application.AdminUserAgg;
using Application.DoctorAgg;

namespace Web.Models
{
    public class AdminViewModel
    {
        public IReadOnlyList<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
        public IReadOnlyList<AdminUserDto> Users { get; set; } = new List<AdminUserDto>();
        public CreateDoctorDto NewDoctor { get; set; } = new();
        public CreateAdminUserDto NewUser { get; set; } = new();
        public bool CanManage { get; set; }
        public bool CanEditDoctor { get; set; }
    }

    public class LoginViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "ایمیل الزامی است")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "رمز عبور الزامی است")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
