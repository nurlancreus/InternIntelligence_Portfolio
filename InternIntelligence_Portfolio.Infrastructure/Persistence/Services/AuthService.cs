using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Token;
using InternIntelligence_Portfolio.Application.DTOs.Auth;
using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Services
{
    public class AuthService(ITokenService tokenService, IUserService userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserService _userService = userService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        public async Task<Result<TokenDTO>> LoginAsync(LoginRequestDTO loginRequest, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);

            if (user is null) return Result<TokenDTO>.Failure(Error.LoginError());

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    return Result<TokenDTO>.Failure(Error.LoginError("You tried too many times while trying to log in. Please try 5 minutes later."));
                }

                return Result<TokenDTO>.Failure(Error.LoginError());
            }

            var tokenResult = await _tokenService.GetTokenDataAsync(user);

            if (tokenResult.IsFailure) return Result<TokenDTO>.Failure(tokenResult.Error);

            var token = tokenResult.Value;

            var updateRefreshTokenResult = await _userService.UpdateUserRefreshToken(user, token.RefreshToken, token.AccessTokenEndDate);

            if (updateRefreshTokenResult.IsFailure) return Result<TokenDTO>.Failure(updateRefreshTokenResult.Error);

            return Result<TokenDTO>.Success(token);
        }

        public async Task<Result<TokenDTO>> RefreshLoginAsync(RefreshLoginRequestDTO refreshLoginRequest, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshLoginRequest.RefreshToken, cancellationToken);

            if (user is null) 
                return Result<TokenDTO>.Failure(Error.NotFoundError("User is not found."));

            var claimsPrincipalResult = _tokenService.GetPrincipalFromAccessToken(refreshLoginRequest.AccessToken);

            if (claimsPrincipalResult.IsFailure) return Result<TokenDTO>.Failure(claimsPrincipalResult.Error);

            var claimsPrincipal = claimsPrincipalResult.Value;

            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != user.Id.ToString()) return Result<TokenDTO>.Failure(Error.TokenError("Invalid token"));

            var newTokenResult = _tokenService.GenerateAccessToken(claimsPrincipal.Claims);

            if (newTokenResult.IsFailure) return Result<TokenDTO>.Failure(newTokenResult.Error);

            var (accessToken, tokenEndDate) = newTokenResult.Value;

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var updateRefreshTokenResult = await _userService.UpdateUserRefreshToken(user, newRefreshToken, tokenEndDate);

            if (updateRefreshTokenResult.IsFailure) return Result<TokenDTO>.Failure(updateRefreshTokenResult.Error);

            var token = new TokenDTO
            {
                AccessToken = accessToken,
                AccessTokenEndDate = tokenEndDate,
                RefreshToken = newRefreshToken,
            };

            return Result<TokenDTO>.Success(token);
        }
    }
}
