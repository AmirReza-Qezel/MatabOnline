using Application.Common;
using Domain.AppointmentAgg;

namespace Application.AppointmentAgg
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IReadOnlyList<Appointment>> GetByDoctorIdAsync(int doctorId);

        // Prevents a patient double-booking the exact same doctor/date/time slot.
        Task<bool> ExistsForDoctorAtAsync(int doctorId, DateTime appointmentDate);
    }
}
