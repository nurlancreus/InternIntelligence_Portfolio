using InternIntelligence_Portfolio.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Storage
{
    public interface IStorage : IEnlistmentNotification
    {
        Task<Result<(string path, string fileName)>> UploadAsync(string path, IFormFile formFile);
        Task<Result<List<(string path, string fileName)>>> UploadMultipleAsync(string path, IFormFileCollection formFiles);
        Task DeleteAsync(string path, string fileName);
        Task DeleteAllAsync(string path);
        Task<Result<List<(string path, string fileName)>>> GetFilesAsync(string path);
        Task<bool> HasFileAsync(string path, string fileName);
    }
}
