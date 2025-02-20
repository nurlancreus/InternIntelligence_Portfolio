using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.Abstractions.Repositories;
using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Services
{
    public class AchievementService(IJwtSession jwtSession, IUnitOfWork unitOfWork) : IAchievementService
    {
        private readonly IJwtSession _jwtSession = jwtSession;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRepository<Achievement> _achievementRepository = unitOfWork.GetRepository<Achievement>();

        public async Task<Result<Guid>> CreateAsync(CreateAchievementRequestDTO createAchievementRequest, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<Guid>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var userIdResult = _jwtSession.GetUserId();
            if (userIdResult.IsFailure) return Result<Guid>.Failure(userIdResult.Error);

            var achievement = Achievement.Create(
                createAchievementRequest.Title,
                createAchievementRequest.Description,
                createAchievementRequest.AchievedAt
            );
            achievement.UserId = userIdResult.Value;

            var isAdded = await _achievementRepository.AddAsync(achievement, cancellationToken);
            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isAdded || !isSaved)
                return Result<Guid>.Failure(Error.UnexpectedError("New achievement could not be created."));

            scope.Complete();
            return Result<Guid>.Success(achievement.Id);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<bool>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var achievement = await _achievementRepository.GetByIdAsync(id, cancellationToken);
            if (achievement is null)
                return Result<bool>.Failure(Error.NotFoundError("Achievement is not found."));

            var isDeleted = _achievementRepository.Delete(achievement);
            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isDeleted || !isSaved)
                return Result<bool>.Failure(Error.UnexpectedError("Achievement could not be deleted."));

            scope.Complete();
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<GetAchievementResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure)
                return Result<IEnumerable<GetAchievementResponseDTO>>.Failure(isSuperAdminResult.Error);

            var achievements = _achievementRepository.Table.AsNoTracking();
            var achievementDtos = await achievements
                .Select(a => new GetAchievementResponseDTO(a))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<GetAchievementResponseDTO>>.Success(achievementDtos);
        }

        public async Task<Result<GetAchievementResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<GetAchievementResponseDTO>.Failure(isSuperAdminResult.Error);

            var achievement = await _achievementRepository.Table
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (achievement is null)
                return Result<GetAchievementResponseDTO>.Failure(Error.NotFoundError("Achievement is not found."));

            var achievementDto = new GetAchievementResponseDTO(achievement);

            return Result<GetAchievementResponseDTO>.Success(achievementDto);
        }

        public async Task<Result<Guid>> UpdateAsync(UpdateAchievementRequestDTO updateAchievementRequest, CancellationToken cancellationToken = default)
        {
            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<Guid>.Failure(isSuperAdminResult.Error);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var achievement = await _achievementRepository.GetByIdAsync(updateAchievementRequest.Id, cancellationToken);
            if (achievement is null)
                return Result<Guid>.Failure(Error.NotFoundError("Achievement is not found."));

            if (!string.IsNullOrEmpty(updateAchievementRequest.Title) && achievement.Title != updateAchievementRequest.Title)
            {
                achievement.Title = updateAchievementRequest.Title;
            }

            if (!string.IsNullOrEmpty(updateAchievementRequest.Description) && achievement.Description != updateAchievementRequest.Description)
            {
                achievement.Description = updateAchievementRequest.Description;
            }

            if (updateAchievementRequest.AchievedAt.HasValue && achievement.AchievedAt != updateAchievementRequest.AchievedAt)
            {
                achievement.AchievedAt = updateAchievementRequest.AchievedAt.Value;
            }

            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (!isSaved)
                return Result<Guid>.Failure(Error.UnexpectedError("Achievement could not be updated."));

            scope.Complete();
            return Result<Guid>.Success(achievement.Id);
        }
    }
}