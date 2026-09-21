using Application.AppointmentAgg;
using Domain.AppointmentAgg;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Appointment>> GetByDoctorIdAsync(int doctorId) =>
            await DbSet
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

        public async Task<bool> ExistsForDoctorAtAsync(int doctorId, DateTime appointmentDate) =>
            await DbSet.AnyAsync(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate);
    }
}
