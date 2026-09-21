using Application.AppointmentAgg;
using Application.ContactMessageAgg;
using Application.DoctorAgg;

namespace Web.Models
{
    // Composes everything the Home/Index page needs into one strongly-typed model.
    // This is a VIEW model - it exists only to serve this page - which is why it
    // lives in Web, not Application. DTOs (DoctorDto, CreateAppointmentDto, ...)
    // stay in Application; this just bundles them for one screen.
    public class HomeViewModel
    {
        public IReadOnlyList<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
        public CreateAppointmentDto AppointmentForm { get; set; } = new();
        public CreateContactMessageDto ContactForm { get; set; } = new();
    }
}
