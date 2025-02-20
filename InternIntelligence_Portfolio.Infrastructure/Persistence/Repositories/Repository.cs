using InternIntelligence_Portfolio.Application.Abstractions.Repositories;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Repositories
{
    public class Repository<T>(AppDbContext appDbContext) : IRepository<T> where T : Base
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public DbSet<T> Table => _appDbContext.Set<T>();

        public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = await Table.AddAsync(entity, cancellationToken);

            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await Table.AddRangeAsync(entities, cancellationToken);

            return true;
        }

        public bool Delete(T entity)
        {
            var entityEntry = Table.Remove(entity);

            return entityEntry.State == EntityState.Deleted;
        }

        public bool Update(T entity)
        {
            var entityEntry = Table.Update(entity);

            return entityEntry.State == EntityState.Modified;
        }

        public bool UpdateRange(IEnumerable<T> entities)
        {
            Table.UpdateRange(entities);

            return true;
        }

        public bool DeleteRange(IEnumerable<T> entities)
        {
            Table.RemoveRange(entities);

            return true;
        }

        public IQueryable<T> GetAll(bool isTracked = true)
        {
            var query = Table.AsQueryable();

            if (!isTracked) query = query.AsNoTracking();

            return query;
        }

        public IQueryable<T> GetAllWhere(Expression<Func<T, bool>> predicate, bool isTracked = true)
        {
            var query = Table.Where(predicate);

            if (!isTracked) query = query.AsNoTracking();

            return query;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool isTracked = true)
        {
            var query = Table.AsQueryable();

            if (!isTracked) query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<T?> GetWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default, bool isTracked = true)
        {
            var query = Table.AsQueryable();

            if (!isTracked) query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
