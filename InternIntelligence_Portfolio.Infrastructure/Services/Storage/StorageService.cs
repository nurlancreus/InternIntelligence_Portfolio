using InternIntelligence_Portfolio.Application.Abstractions.Services.Storage;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using InternIntelligence_Portfolio.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Storage
{
    public class StorageService(IStorage storage) : IStorageService
    {
        private readonly IStorage _storage = storage;

        public StorageType StorageName
        {
            get
            {
                var storageName = _storage.GetType().Name.Replace("Storage", string.Empty);

                if (Enum.TryParse(storageName, out StorageType storageType)) return storageType;    
                else throw new InvalidOperationException("Cannot parse StorageType enum");
            }
        }

        public void Commit(Enlistment enlistment) => _storage.Commit(enlistment);

        public async Task<Result<bool>> DeleteAllAsync(string path)
          => await _storage.DeleteAllAsync(path);


        public async Task<Result<bool>> DeleteAsync(string path, string fileName)
            => await _storage.DeleteAsync(path, fileName);

        public async Task DeleteMultipleAsync<T>(ICollection<T> files) where T : ApplicationFile
        {
            foreach (var file in files)
            {
                await _storage.DeleteAsync(file.Path, file.Name);
            }
        }

        public Task<Result<List<(string path, string fileName)>>> GetFilesAsync(string path)
            => _storage.GetFilesAsync(path);

        public Task<bool> HasFileAsync(string path, string fileName)
            => _storage.HasFileAsync(path, fileName);

        public void InDoubt(Enlistment enlistment) => _storage.InDoubt(enlistment);

        public void Prepare(PreparingEnlistment preparingEnlistment) => _storage.Prepare(preparingEnlistment);

        public void Rollback(Enlistment enlistment) => _storage.Rollback(enlistment);

        public Task<Result<(string path, string fileName)>> UploadAsync(string path, IFormFile formFile)
         => _storage.UploadAsync(path, formFile);

        public Task<Result<List<(string path, string fileName)>>> UploadMultipleAsync(string path, IFormFileCollection files)
            => _storage.UploadMultipleAsync(path, files);
    }
}
