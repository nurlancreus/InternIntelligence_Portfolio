using InternIntelligence_Portfolio.Application.Abstractions.Services.Storage.Local;
using InternIntelligence_Portfolio.Application.Helpers;
using InternIntelligence_Portfolio.Domain.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Storage.Local
{
    public class LocalStorage(IWebHostEnvironment webHostEnvironment) : ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly List<(string path, string fileName)> _uploadedFiles = []; // Track uploaded files for rollback

        public async Task<Result<bool>> DeleteAsync(string path, string fileName)
        {
            try
            {
                if (await HasFileAsync(path, fileName))
                {
                    File.Delete(GetFullPath(path, fileName));
                }

                return await Task.FromResult(Result<bool>.Success(true));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(Error.UnexpectedError($"Unexpected error happened while deleting the file: {ex.Message}"));
            }
        }

        public async Task<Result<List<(string path, string fileName)>>> GetFilesAsync(string path)
        {
            DirectoryInfo directory = new(GetFullPath(path));
            var files = directory.GetFiles().Select(f => (directory.Name, f.Name)).ToList();
            return await Task.FromResult(Result<List<(string path, string fileName)>>.Success(files));
        }

        public async Task<bool> HasFileAsync(string path, string fileName)
        {
            string filePath = GetFullPath(path, fileName);
            return await Task.FromResult(File.Exists(filePath));
        }

        public async Task<Result<(string path, string fileName)>> UploadAsync(string path, IFormFile formFile)
        {
            string uploadPath = GetFullPath(path);
            FileHelpers.EnsureDirectoryExists(uploadPath);

            string newFileName = await FileHelpers.RenameFileAsync(path, formFile.FileName, HasFileAsync);

            string fullPath = Path.Combine(uploadPath, newFileName);
            var isCopiedResult = await CopyFileAsync(fullPath, formFile);

            if (isCopiedResult.IsFailure)
            {
                return Result<(string path, string fileName)>.Failure(Error.UnexpectedError("File upload failed."));
            }

            // Track the uploaded file for potential rollback
            _uploadedFiles.Add((path, newFileName));

            // Enlist the operation in the current transaction
            EnlistInTransaction();

            return Result<(string path, string fileName)>.Success((path, newFileName));
        }

        public async Task<Result<List<(string path, string fileName)>>> UploadMultipleAsync(string path, IFormFileCollection formFiles)
        {
            if (formFiles.Count == 0)
            {
                return Result<List<(string path, string fileName)>>.Failure(Error.BadRequestError("No files uploaded."));
            }

            string uploadPath = GetFullPath(path);
            FileHelpers.EnsureDirectoryExists(uploadPath);

            var uploadedFiles = new List<(string path, string fileName)>();

            foreach (var formFile in formFiles)
            {
                string newFileName = await FileHelpers.RenameFileAsync(path, formFile.FileName, HasFileAsync);
                string fullPath = Path.Combine(uploadPath, newFileName);

                var isCopiedResult = await CopyFileAsync(fullPath, formFile);

                if (isCopiedResult.IsFailure)
                {
                    await RollbackUploads(uploadedFiles);
                    return Result<List<(string path, string fileName)>>.Failure(Error.UnexpectedError("File upload failed. Rolling back previous uploads."));
                }

                uploadedFiles.Add((path, newFileName));
                _uploadedFiles.Add((path, newFileName)); // Track for rollback
            }

            // Enlist the operation in the current transaction
            EnlistInTransaction();

            return Result<List<(string path, string fileName)>>.Success(uploadedFiles);
        }

        public async Task<Result<bool>> DeleteAllAsync(string path)
        {
            try
            {
                DirectoryInfo di = new(GetFullPath(path));
                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }

                return await Task.FromResult(Result<bool>.Success(true));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(Error.UnexpectedError($"Unexpected error happened while deleting files: {ex.Message}"));
            }
        }

        private string GetFullPath(string path, string? fileName = null)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, path, fileName ?? string.Empty);
        }

        private static async Task<Result<bool>> CopyFileAsync(string path, IFormFile formFile)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(Error.UnexpectedError($"Error copying file: {ex.Message}"));
            }
        }

        private async Task RollbackUploads(List<(string path, string fileName)> uploadedFiles)
        {
            foreach (var (path, fileName) in uploadedFiles)
            {
                string filePath = GetFullPath(path, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            await Task.CompletedTask;
        }

        private void EnlistInTransaction()
        {
            // Enlist this service in the current transaction
            Transaction.Current?.EnlistVolatile(this, EnlistmentOptions.None);
        }

        // IEnlistmentNotification Implementation for TransactionScope
        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            // No specific preparation needed for local file operations, so mark as prepared
            preparingEnlistment.Prepared();
        }

        public void Commit(Enlistment enlistment)
        {
            // Clear the uploaded files list because the transaction succeeded
            _uploadedFiles.Clear();
            enlistment.Done();
        }

        public async void Rollback(Enlistment enlistment)
        {
            // Delete all files uploaded during the transaction if it fails
            await RollbackUploads(_uploadedFiles);
            _uploadedFiles.Clear();
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            // Handle any "in doubt" cases, though they are rare
            enlistment.Done();
        }
    }
}
