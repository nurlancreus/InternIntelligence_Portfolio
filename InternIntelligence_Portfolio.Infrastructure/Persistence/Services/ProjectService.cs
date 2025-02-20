using System.Transactions;
using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.Abstractions.Repositories;
using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Storage;
using InternIntelligence_Portfolio.Application.DTOs.Project;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Services
{
    public class ProjectService(IUnitOfWork unitOfWork, IJwtSession jwtSession, IStorageService storageService) : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtSession _jwtSession = jwtSession;
        private readonly IStorageService _storageService = storageService;
        private readonly IRepository<Project> _projectRepository = unitOfWork.GetRepository<Project>();

        public async Task<Result<Guid>> CreateAsync(CreateProjectRequestDTO createProjectRequest, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<Guid>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var userIdResult = _jwtSession.GetUserId();
            if (userIdResult.IsFailure) return Result<Guid>.Failure(userIdResult.Error);

            if (!UrlValidator.IsUrlValid(createProjectRequest.RepoUrl))
                return Result<Guid>.Failure(Error.BadRequestError("Repo URL is not in the correct format."));

            if (!UrlValidator.IsUrlValid(createProjectRequest.LiveUrl))
                return Result<Guid>.Failure(Error.BadRequestError("Live URL is not in the correct format."));

            var project = Project.Create(createProjectRequest.Name, createProjectRequest.Description, createProjectRequest.RepoUrl, createProjectRequest.LiveUrl);
            project.UserId = userIdResult.Value;

            if (createProjectRequest.ProjectCoverImageFile is not null)
            {
                var uploadResult = await _storageService.UploadAsync(DomainConstants.Project.ProjectCoverImageContainerName, createProjectRequest.ProjectCoverImageFile);

                if (uploadResult.IsFailure) return Result<Guid>.Failure(uploadResult.Error);

                var (path, fileName) = uploadResult.Value;

                var coverPhoto = ProjectCoverImageFile.Create(fileName, path, _storageService.StorageName);

                project.CoverImageFile = coverPhoto;
            }

            var isAdded = await _projectRepository.AddAsync(project, cancellationToken);
            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isAdded || !isSaved)
                return Result<Guid>.Failure(Error.UnexpectedError("New project could not be created."));

            scope.Complete();
            return Result<Guid>.Success(project.Id);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<bool>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var project = await _projectRepository.GetByIdAsync(id, cancellationToken);
            if (project is null)
                return Result<bool>.Failure(Error.NotFoundError("Project is not found."));

            var isDeleted = _projectRepository.Delete(project);

            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isDeleted || !isSaved)
                return Result<bool>.Failure(Error.UnexpectedError("Project could not be deleted."));

            if (project.CoverImageFile is not null)
            {
                var deleteResult = await _storageService.DeleteAsync(DomainConstants.Project.ProjectCoverImageContainerName, project.CoverImageFile.Name);
                if (deleteResult.IsFailure)
                    return Result<bool>.Failure(deleteResult.Error);
            }

            scope.Complete();
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<GetProjectResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) 
                return Result<IEnumerable<GetProjectResponseDTO>>.Failure(isSuperAdminResult.Error);

            var projects = _projectRepository.Table
                                .Include(p => p.CoverImageFile)
                                .AsNoTracking();

            var projectDtos = await projects.Select(p => new GetProjectResponseDTO(p)).ToListAsync(cancellationToken);

            return Result<IEnumerable<GetProjectResponseDTO>>.Success(projectDtos);
        }

        public async Task<Result<GetProjectResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<GetProjectResponseDTO>.Failure(isSuperAdminResult.Error);

            var project = await _projectRepository.Table
                                .Include(p => p.CoverImageFile)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (project is null)
                return Result<GetProjectResponseDTO>.Failure(Error.NotFoundError("Project is not found."));

            var projectDto = new GetProjectResponseDTO(project);

            return Result<GetProjectResponseDTO>.Success(projectDto);
        }

        public async Task<Result<Guid>> UpdateAsync(UpdateProjectRequestDTO updateProjectRequest, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<Guid>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var project = await _projectRepository.GetByIdAsync(updateProjectRequest.Id, cancellationToken);
            if (project is null)
                return Result<Guid>.Failure(Error.NotFoundError("Project is not found."));

            if (!string.IsNullOrEmpty(updateProjectRequest.Name) && project.Name != updateProjectRequest.Name)
            {
                project.Name = updateProjectRequest.Name;
            }

            if (!string.IsNullOrEmpty(updateProjectRequest.Description) && project.Description != updateProjectRequest.Description)
            {
                project.Description = updateProjectRequest.Description;
            }

            if (!string.IsNullOrEmpty(updateProjectRequest.RepoUrl) && project.RepoUrl != updateProjectRequest.RepoUrl)
            {
                if (!UrlValidator.IsUrlValid(updateProjectRequest.RepoUrl))
                    return Result<Guid>.Failure(Error.BadRequestError("Repo URL is not in the correct format."));

                project.RepoUrl = updateProjectRequest.RepoUrl;
            }

            if (!string.IsNullOrEmpty(updateProjectRequest.LiveUrl) && project.LiveUrl != updateProjectRequest.LiveUrl)
            {
                if (!UrlValidator.IsUrlValid(updateProjectRequest.LiveUrl))
                    return Result<Guid>.Failure(Error.BadRequestError("Live URL is not in the correct format."));

                project.LiveUrl = updateProjectRequest.LiveUrl;
            }

            ProjectCoverImageFile? oldCoverPhotoFile = null;

            if (updateProjectRequest.ProjectCoverImageFile is not null)
            {
                oldCoverPhotoFile = project.CoverImageFile;

                var uploadResult = await _storageService.UploadAsync(DomainConstants.Project.ProjectCoverImageContainerName, updateProjectRequest.ProjectCoverImageFile);

                if (uploadResult.IsFailure) return Result<Guid>.Failure(uploadResult.Error);

                var (path, fileName) = uploadResult.Value;
                var coverPhoto = ProjectCoverImageFile.Create(fileName, path, _storageService.StorageName);
                project.CoverImageFile = coverPhoto;
            }

            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isSaved)
                return Result<Guid>.Failure(Error.UnexpectedError("Project could not be updated."));

            if (oldCoverPhotoFile is not null)
            {
                var deleteResult = await _storageService.DeleteAsync(DomainConstants.Project.ProjectCoverImageContainerName, oldCoverPhotoFile.Name);

                if (deleteResult.IsFailure) return Result<Guid>.Failure(deleteResult.Error);
            }

            scope.Complete();
            return Result<Guid>.Success(project.Id);
        }
    }
}