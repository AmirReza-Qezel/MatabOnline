using System.ComponentModel.DataAnnotations;

namespace Application.ContactMessageAgg
{
    public record ContactMessageDto(
        int Id, string FullName, string Email, string Message, bool IsRead, DateTime CreatedAt);

    public class CreateContactMessageDto
    {
        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "متن پیام الزامی است")]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;
    }
}
