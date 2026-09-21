using Application.Common;
using Domain.DoctorAgg;

namespace Application.DoctorAgg
{
    // Lives in Application because it only depends on abstractions (IUnitOfWork,
    // IDoctorRepository) - no EF Core, no ASP.NET Core references here.
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IUnitOfWork unitOfWork, IDoctorRepository doctorRepository)
        {
            _unitOfWork = unitOfWork;
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            return doctor is null ? null : MapToDto(doctor);
        }

        public async Task<IReadOnlyList<DoctorDto>> GetAllAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return doctors.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<DoctorDto>> SearchAsync(string? specialty, string? nameQuery)
        {
            var doctors = await _doctorRepository.SearchAsync(specialty, nameQuery);
            return doctors.Select(MapToDto).ToList();
        }

        public async Task<PagedResult<DoctorDto>> GetPagedAsync(int pageIndex, int pageSize)
        {
            var (items, total) = await _doctorRepository.GetPagedAsync(
                pageIndex, pageSize, orderBy: q => q.OrderBy(d => d.FullName));

            return new PagedResult<DoctorDto>
            {
                Items = items.Select(MapToDto).ToList(),
                TotalCount = total,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<int> CreateAsync(CreateDoctorDto dto)
        {
            // Uses the domain constructor - not an object initializer -
            // so any invariant you later add to Doctor's ctor is enforced here too.
            var doctor = new Doctor(dto.FullName, dto.Specialty, dto.Info, dto.ClinicPhoneNumber);

            await _doctorRepository.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return doctor.Id;
        }

        public async Task UpdateAsync(UpdateDoctorDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"پزشکی با شناسه {dto.Id} یافت نشد");

            doctor.Edit(dto.FullName, dto.Specialty, dto.Info, dto.ClinicPhoneNumber);

            _doctorRepository.Update(doctor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"پزشکی با شناسه {id} یافت نشد");

            _doctorRepository.Remove(doctor); // soft delete
            await _unitOfWork.SaveChangesAsync();
        }

        private static DoctorDto MapToDto(Doctor d) => new(
            d.Id, d.FullName, d.Specialty, d.Info, d.ClinicPhoneNumber,
            d.Appointments?.Count ?? 0);
    }
}
