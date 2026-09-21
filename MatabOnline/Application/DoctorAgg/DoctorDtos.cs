using System.ComponentModel.DataAnnotations;

namespace Application.DoctorAgg
{
    public record DoctorDto(
        int Id,
        string FullName,
        string Specialty,
        string Info,
        string ClinicPhoneNumber,
        int AppointmentCount);

    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "نام پزشک الزامی است")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "تخصص الزامی است")]
        [StringLength(100)]
        public string Specialty { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Info { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره تماس مطب الزامی است")]
        [Phone]
        public string ClinicPhoneNumber { get; set; } = string.Empty;
    }

    public class UpdateDoctorDto : CreateDoctorDto
    {
        public int Id { get; set; }
    }
}
