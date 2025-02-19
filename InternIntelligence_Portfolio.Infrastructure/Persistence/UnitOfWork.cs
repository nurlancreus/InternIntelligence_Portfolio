using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.Abstractions.Repositories;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Repositories;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence
{
    public class UnitOfWork(AppDbContext appDbContext) : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public void Dispose()
        {
            _appDbContext.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : Base
        {
            return new Repository<T>(_appDbContext);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changedRecords = await _appDbContext.SaveChangesAsync(cancellationToken);

            return changedRecords > 0;
        }
    }
}
