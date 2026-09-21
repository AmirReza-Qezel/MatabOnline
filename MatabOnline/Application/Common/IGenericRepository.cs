using Domain.Common;
using System.Linq.Expressions;

namespace Application.Common
{
    // One generic repository interface for every entity that derives from BaseEntity.
    // Entity-specific repos (IDoctorRepository, IAppointmentRepository) extend this
    // and only add the methods that truly need custom EF Core queries (eager loading etc).
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);

        Task<(IReadOnlyList<T> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity);

        // Marks the entity as modified. Used after mutating it via its own
        // domain methods (e.g. doctor.Edit(...)) - never pass in a "fresh" DTO-mapped entity.
        void Update(T entity);

        // Soft delete: calls entity.Remove() (sets IsDeleted = true) and marks it modified.
        // Does NOT physically delete the row.
        void Remove(T entity);

        // Escape hatch for the rare case you actually need a hard delete.
        void HardDelete(T entity);

        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
