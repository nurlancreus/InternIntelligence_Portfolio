using InternIntelligence_Portfolio.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Repositories
{
    public interface IRepository<T> where T : Base
    {
        DbSet<T> Table { get; }

        // Read
        IQueryable<T> GetAll(bool isTracked = true);
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>> predicate, bool isTracked = true);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool isTracked = true);
        Task<T?> GetWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, bool isTracked = true);

        // Write
        Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        bool Delete(T entity);
        bool DeleteRange(IEnumerable<T> entities);
        bool Update(T entity);
        bool UpdateRange(IEnumerable<T> entities);
    }
}
