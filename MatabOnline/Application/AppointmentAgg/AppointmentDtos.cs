using System.ComponentModel.DataAnnotations;

namespace Application.AppointmentAgg
{
    public record AppointmentDto(
        int Id,
        int DoctorId,
        string DoctorFullName,
        string PatientName,
        string PatientPhoneNumber,
        DateTime AppointmentDate);

    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "انتخاب پزشک الزامی است")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "نام بیمار الزامی است")]
        [StringLength(100)]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره تماس الزامی است")]
        [Phone(ErrorMessage = "شماره تماس معتبر نیست")]
        public string PatientPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاریخ نوبت الزامی است")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }
    }

    public class UpdateAppointmentDto : CreateAppointmentDto
    {
        public int Id { get; set; }
    }
}
