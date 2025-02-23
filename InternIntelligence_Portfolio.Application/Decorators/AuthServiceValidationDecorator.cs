using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Auth;
using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Decorators
{
    public class AuthServiceValidationDecorator : IAuthService
    {
        private readonly IAuthService _innerAuthService;
        private readonly RequestValidator _requestValidator;

        public AuthServiceValidationDecorator(IAuthService innerAuthService, RequestValidator requestValidator)
        {
            _innerAuthService = innerAuthService;
            _requestValidator = requestValidator;
        }

        public async Task<Result<TokenDTO>> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request);
            if (!validationResult.IsSuccess)
                return Result<TokenDTO>.Failure(validationResult.Error);

            return await _innerAuthService.LoginAsync(request, cancellationToken);
        }

        public async Task<Result<TokenDTO>> RefreshLoginAsync(RefreshLoginRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request);
            if (!validationResult.IsSuccess)
                return Result<TokenDTO>.Failure(validationResult.Error);

            return await _innerAuthService.RefreshLoginAsync(request, cancellationToken);
        }
    }
}
