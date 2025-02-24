using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Auth;
using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Decorators
{
    public class AuthServiceValidationDecorator(IAuthService innerAuthService, RequestValidator requestValidator) : IAuthService
    {
        private readonly IAuthService _innerAuthService = innerAuthService;
        private readonly RequestValidator _requestValidator = requestValidator;

        public async Task<Result<TokenDTO>> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsFailure)
                return Result<TokenDTO>.Failure(validationResult.Error);

            return await _innerAuthService.LoginAsync(request, cancellationToken);
        }

        public async Task<Result<TokenDTO>> RefreshLoginAsync(RefreshLoginRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsFailure)
                return Result<TokenDTO>.Failure(validationResult.Error);

            return await _innerAuthService.RefreshLoginAsync(request, cancellationToken);
        }
    }
}
