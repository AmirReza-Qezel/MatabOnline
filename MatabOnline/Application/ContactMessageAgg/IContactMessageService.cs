namespace Application.ContactMessageAgg
{
    public interface IContactMessageService
    {
        Task<IReadOnlyList<ContactMessageDto>> GetAllAsync();
        Task CreateAsync(CreateContactMessageDto dto);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }
}
