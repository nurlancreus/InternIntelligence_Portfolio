using InternIntelligence_Portfolio.Application.DTOs.Auth;
using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<Result<TokenDTO>> LoginAsync(LoginRequestDTO loginRequest, CancellationToken cancellationToken = default);
        Task<Result<TokenDTO>> RefreshLoginAsync(RefreshLoginRequestDTO refreshLoginRequest, CancellationToken cancellationToken = default);
    }
}
