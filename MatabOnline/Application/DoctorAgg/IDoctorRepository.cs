using Application.Common;
using Domain.DoctorAgg;

namespace Application.DoctorAgg
{
    // Extends the generic repository - only adding what actually needs a custom query.
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        // Eager-loads Appointments - used by the admin doctor detail page.
        Task<Doctor?> GetWithAppointmentsAsync(int doctorId);

        Task<IReadOnlyList<Doctor>> SearchAsync(string? specialty, string? nameQuery);
    }
}
