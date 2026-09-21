using Application.Common;
using Application.DoctorAgg;
using Domain.AppointmentAgg;

namespace Application.AppointmentAgg
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository)
        {
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.FirstOrDefaultAsync(
                a => a.Id == id, a => a.Doctor);

            return appointment is null ? null : MapToDto(appointment);
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetPagedAsync(
                1, int.MaxValue,
                orderBy: q => q.OrderByDescending(a => a.AppointmentDate),
                includes: a => a.Doctor);

            return appointments.Items.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetByDoctorIdAsync(int doctorId)
        {
            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId);
            return appointments.Select(MapToDto).ToList();
        }

        public async Task<(int? Id, string? Error)> CreateAsync(CreateAppointmentDto dto)
        {
            var doctorExists = await _doctorRepository.AnyAsync(d => d.Id == dto.DoctorId);
            if (!doctorExists)
                return (null, "پزشک مورد نظر یافت نشد");

            var slotTaken = await _appointmentRepository.ExistsForDoctorAtAsync(dto.DoctorId, dto.AppointmentDate);
            if (slotTaken)
                return (null, "این بازه زمانی قبلاً برای این پزشک رزرو شده است");

            var appointment = new Appointment(
                dto.DoctorId, dto.PatientName, dto.PatientPhoneNumber, dto.AppointmentDate);

            await _appointmentRepository.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return (appointment.Id, null);
        }

        public async Task UpdateAsync(UpdateAppointmentDto dto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"نوبتی با شناسه {dto.Id} یافت نشد");

            appointment.Edit(dto.DoctorId, dto.PatientName, dto.PatientPhoneNumber, dto.AppointmentDate);

            _appointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"نوبتی با شناسه {id} یافت نشد");

            _appointmentRepository.Remove(appointment); // soft delete
            await _unitOfWork.SaveChangesAsync();
        }

        private static AppointmentDto MapToDto(Appointment a) => new(
            a.Id, a.DoctorId, a.Doctor?.FullName ?? string.Empty,
            a.PatientName, a.PatientPhoneNumber, a.AppointmentDate);
    }
}
