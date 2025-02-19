using InternIntelligence_Portfolio.Domain.Entities.Files;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Storage
{
    public interface IStorageService : IStorage
    {
        public StorageType StorageName { get; }
        Task DeleteMultipleAsync<T>(ICollection<T> files) where T : ApplicationFile;
    }
}
