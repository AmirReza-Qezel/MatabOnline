using Application.Common;

namespace Application.DoctorAgg
{
    public interface IDoctorService
    {
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<DoctorDto>> GetAllAsync();
        Task<IReadOnlyList<DoctorDto>> SearchAsync(string? specialty, string? nameQuery);
        Task<PagedResult<DoctorDto>> GetPagedAsync(int pageIndex, int pageSize);
        Task<int> CreateAsync(CreateDoctorDto dto);
        Task UpdateAsync(UpdateDoctorDto dto);
        Task DeleteAsync(int id);
    }
}
