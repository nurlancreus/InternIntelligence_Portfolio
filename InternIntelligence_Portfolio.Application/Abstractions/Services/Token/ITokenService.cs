using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using System.Security.Claims;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Token
{
    public interface ITokenService
    {
        Task<Result<(string accessToken, DateTime tokenEndDate)>> GenerateAccessTokenAsync(ApplicationUser user);
        Result<(string accessToken, DateTime tokenEndDate)> GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        Task<Result<TokenDTO>> GetTokenDataAsync(ApplicationUser user);
        Result<ClaimsPrincipal> GetPrincipalFromAccessToken(string? accessToken);
    }
}
