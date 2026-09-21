using Domain.Common;

namespace Application.Common
{
    public interface IUnitOfWork : IDisposable
    {
        // Generic access for entities that don't need a custom repository.
        IGenericRepository<T> Repository<T>() where T : BaseEntity;

        Task<int> SaveChangesAsync();
    }
}
