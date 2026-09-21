namespace Application.AppointmentAgg
{
    public interface IAppointmentService
    {
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<AppointmentDto>> GetAllAsync();
        Task<IReadOnlyList<AppointmentDto>> GetByDoctorIdAsync(int doctorId);

        // Returns the new appointment id, or null + error message if the slot is taken.
        Task<(int? Id, string? Error)> CreateAsync(CreateAppointmentDto dto);
        Task UpdateAsync(UpdateAppointmentDto dto);
        Task DeleteAsync(int id);
    }
}
