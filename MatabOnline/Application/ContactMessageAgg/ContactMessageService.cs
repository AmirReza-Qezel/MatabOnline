using Application.Common;
using Domain.ContactMessageAgg;

namespace Application.ContactMessageAgg
{
    // No entity-specific repository needed here - IGenericRepository<ContactMessage>
    // covers everything this aggregate needs. This is the "and so on" case:
    // don't create a repository interface/class just to satisfy a convention.
    public class ContactMessageService : IContactMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactMessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ContactMessageDto>> GetAllAsync()
        {
            var messages = await _unitOfWork.Repository<ContactMessage>().GetAllAsync();
            return messages
                .OrderByDescending(m => m.CreatedAt)
                .Select(MapToDto)
                .ToList();
        }

        public async Task CreateAsync(CreateContactMessageDto dto)
        {
            var message = new ContactMessage(dto.FullName, dto.Email, dto.Message, isRead: false);
            await _unitOfWork.Repository<ContactMessage>().AddAsync(message);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var repo = _unitOfWork.Repository<ContactMessage>();
            var message = await repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"پیامی با شناسه {id} یافت نشد");

            message.Edit(message.FullName, message.Email, message.Message, isRead: true);
            repo.Update(message);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<ContactMessage>();
            var message = await repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"پیامی با شناسه {id} یافت نشد");

            repo.Remove(message);
            await _unitOfWork.SaveChangesAsync();
        }

        private static ContactMessageDto MapToDto(ContactMessage m) =>
            new(m.Id, m.FullName, m.Email, m.Message, m.IsRead, m.CreatedAt);
    }
}
