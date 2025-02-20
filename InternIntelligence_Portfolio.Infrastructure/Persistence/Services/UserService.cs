using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Storage;
using InternIntelligence_Portfolio.Application.DTOs.User;
using InternIntelligence_Portfolio.Application.Helpers;
using InternIntelligence_Portfolio.Application.Options.Token;
using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Transactions;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, IOptions<TokenSettings> options, IJwtSession jwtSession, IStorageService storageService) : IUserService
    {
        private readonly IJwtSession _jwtSession = jwtSession;
        private readonly IStorageService _storageService = storageService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RefreshSettings _refreshSettings = options.Value.Refresh;

        public async Task<Result<bool>> ChangeProfilePictureAsync(ChangeProfilePictureRequestDTO changeProfilePictureRequest, CancellationToken cancellationToken = default)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var userIdResult = _jwtSession.GetUserId();

            if (userIdResult.IsFailure) return Result<bool>.Failure(userIdResult.Error);

            var user = await _userManager.Users
                                .Include(u => u.ProfilePictureFile)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.Id == userIdResult.Value, cancellationToken);

            if (user is null) return Result<bool>.Failure(Error.NotFoundError("User is not found."));

            var uploadResult = await _storageService.UploadAsync(DomainConstants.User.UserProfilePictureContainerName, changeProfilePictureRequest.NewProfilePictureFile);

            if (uploadResult.IsFailure) return Result<bool>.Failure(uploadResult.Error);

            var (path, fileName) = uploadResult.Value;

            var newPictureFile = UserProfilePictureFile.Create(fileName, path, _storageService.StorageName);

            var oldProfilePhotoFile = user.ProfilePictureFile;

            user.ProfilePictureFile = newPictureFile;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded) return Result<bool>.Failure(Error.UnexpectedError($"Unexpected error happened while updating user: {ResponseHelpers.GetResultErrorsMessage(updateResult)}"));

            if (oldProfilePhotoFile is not null)
            {
                var deleteResult = await _storageService.DeleteAsync(DomainConstants.User.UserProfilePictureContainerName, oldProfilePhotoFile.Name);

                if (deleteResult.IsFailure) return Result<bool>.Failure(deleteResult.Error);
            }

            scope.Complete();
            return Result<bool>.Success(updateResult.Succeeded);
        }

        public async Task<Result<GetUserResponseDTO>> GetMeAsync(CancellationToken cancellationToken = default)
        {
            var userIdResult = _jwtSession.GetUserId();

            if (userIdResult.IsFailure) return Result<GetUserResponseDTO>.Failure(userIdResult.Error);

            var user = await _userManager.Users
                                .Include(u => u.Projects)
                                    .ThenInclude(p => p.CoverImageFile)
                                .Include(u => u.Skills)
                                .Include(u => u.Achievements)
                                .Include(u => u.ProfilePictureFile)
                                .FirstOrDefaultAsync(u => u.Id == userIdResult.Value, cancellationToken);

            if (user is null) return Result<GetUserResponseDTO>.Failure(Error.NotFoundError("User is not found."));

            return Result<GetUserResponseDTO>.Success(new GetUserResponseDTO(user));
        }

        public async Task<Result<bool>> UpdateUserRefreshToken(ApplicationUser user, string refreshToken, DateTime accessTokenEndDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenEndDate.AddMinutes(_refreshSettings.RefreshTokenLifeTimeInMinutes);

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded) return Result<bool>.Failure(Error.UnexpectedError($"Unexpected error happened while updating user: {ResponseHelpers.GetResultErrorsMessage(updateResult)}"));

            return Result<bool>.Success(updateResult.Succeeded);
        }
    }
}
