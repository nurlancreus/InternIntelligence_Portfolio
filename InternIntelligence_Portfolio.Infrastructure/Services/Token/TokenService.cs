using InternIntelligence_Portfolio.Application.Abstractions.Services.Token;
using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Application.Options.Token;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Token
{
    public class TokenService(IOptions<TokenSettings> options, UserManager<ApplicationUser> userManager) : ITokenService
    {
        private readonly AccessSettings _accessSettings = options.Value.Access;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<(string accessToken, DateTime tokenEndDate)>> GenerateAccessTokenAsync(ApplicationUser user)
        {
            try
            {
                var claims = await CreateClaimsAsync(user);
                return GenerateAccessToken(claims);
            }
            catch (Exception ex)
            {
                return Result<(string accessToken, DateTime tokenEndDate)>.Failure(Error.UnexpectedError($"Error happened while generating token: {ex.Message}"));
            }
        }

        private async Task<List<Claim>> CreateClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName!), // Name claim (username)
                new(ClaimTypes.GivenName, user.FirstName!),
                new(ClaimTypes.Surname, user.LastName!),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (user id)
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique ID (JTI)
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()), // Issued at (Unix timestamp)
                new(ClaimTypes.NameIdentifier, user.Id.ToString()), // Unique name identifier of the user (id)
                new(ClaimTypes.Email, user.Email!), // Email of the user
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        public Result<(string accessToken, DateTime tokenEndDate)> GenerateAccessToken(IEnumerable<Claim> claims)
        {
            try
            {
                var tokenEndDate = DateTime.UtcNow.AddMinutes(_accessSettings.AccessTokenLifeTimeInMinutes);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessSettings.SecurityKey));

                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var securityToken = new JwtSecurityToken(
                    audience: _accessSettings.Audience,
                    issuer: _accessSettings.Issuer,
                    expires: tokenEndDate,
                    notBefore: DateTime.UtcNow,
                    signingCredentials: signingCredentials,
                    claims: claims
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                var accessToken = tokenHandler.WriteToken(securityToken);

                return Result<(string accessToken, DateTime tokenEndDate)>.Success((accessToken, tokenEndDate));
            }
            catch (Exception ex)
            {
                return Result<(string accessToken, DateTime tokenEndDate)>.Failure(Error.UnexpectedError($"Error happened while generating token: {ex.Message}"));
            }
        }

        public string GenerateRefreshToken()
        {
            byte[] number = new byte[64];

            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);

            return Convert.ToBase64String(number);
        }

        public Result<ClaimsPrincipal> GetPrincipalFromAccessToken(string? accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Result<ClaimsPrincipal>.Failure(Error.TokenError("Access token is null or empty."));
            }

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _accessSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = _accessSettings.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessSettings.SecurityKey)),

                ValidateLifetime = false //should be false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return Result<ClaimsPrincipal>.Failure(Error.TokenError("Invalid token"));
            }

            return Result<ClaimsPrincipal>.Success(principal);
        }

        public async Task<Result<TokenDTO>> GetTokenDataAsync(ApplicationUser user)
        {
            var accessTokenResult = await GenerateAccessTokenAsync(user);

            if (accessTokenResult.IsFailure) return Result<TokenDTO>.Failure(accessTokenResult.Error);

            var refreshToken = GenerateRefreshToken();

            var (accessToken, tokenEndDate) = accessTokenResult.Value;

            var token = new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenEndDate = tokenEndDate,
            };

            return Result<TokenDTO>.Success(token);
        }
    }
}
