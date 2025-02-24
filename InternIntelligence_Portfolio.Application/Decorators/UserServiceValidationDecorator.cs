using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.User;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Identity;

namespace InternIntelligence_Portfolio.Application.Decorators
{
    public class UserServiceValidationDecorator(IUserService innerUserService, RequestValidator requestValidator) : IUserService
    {
        private readonly IUserService _innerUserService = innerUserService;
        private readonly RequestValidator _requestValidator = requestValidator;

        public async Task<Result<bool>> ChangeProfilePictureAsync(ChangeProfilePictureRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsFailure)
                return Result<bool>.Failure(validationResult.Error);

            return await _innerUserService.ChangeProfilePictureAsync(request, cancellationToken);
        }

        public async Task<Result<GetUserResponseDTO>> GetMeAsync(CancellationToken cancellationToken = default)
        {
            return await _innerUserService.GetMeAsync(cancellationToken);
        }

        public async Task<Result<bool>> UpdateUserRefreshTokenAsync(ApplicationUser user, string refreshToken, DateTime accessTokenEndDate)
        {
            return await _innerUserService.UpdateUserRefreshTokenAsync(user, refreshToken, accessTokenEndDate);
        }
    }
}
