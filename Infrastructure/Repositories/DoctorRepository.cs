using Application.DoctorAgg;
using Domain.DoctorAgg;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    // Inherits the generic repository's CRUD - only adds the two queries
    // that genuinely need Doctor-specific EF Core knowledge.
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Doctor?> GetWithAppointmentsAsync(int doctorId) =>
            await DbSet
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == doctorId);

        public async Task<IReadOnlyList<Doctor>> SearchAsync(string? specialty, string? nameQuery)
        {
            var query = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(specialty))
                query = query.Where(d => d.Specialty == specialty);

            if (!string.IsNullOrWhiteSpace(nameQuery))
                query = query.Where(d => d.FullName.Contains(nameQuery));

            return await query.ToListAsync();
        }
    }
}
