using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Sessions
{
    public class JwtSession(IHttpContextAccessor httpContextAccessor) : IJwtSession
    {
        private readonly ClaimsPrincipal? _claimsPrincipal = httpContextAccessor.HttpContext?.User;
        public Result<Guid> GetUserId()
        {
            var isAuthResult = IsAuth();

            if (isAuthResult.IsFailure) return Result<Guid>.Failure(isAuthResult.Error);

            var userId = _claimsPrincipal!.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) return Result<Guid>.Failure(Error.UnauthorizedError("UserId claim is null"));

            return Result<Guid>.Success(Guid.Parse(userId));
        }

        public Result<bool> ValidateIfSuperAdmin()
        {
            var isAuthResult = IsAuth();

            if (isAuthResult.IsFailure) return Result<bool>.Failure(isAuthResult.Error);

            var roles = _claimsPrincipal!.FindAll(ClaimTypes.Role);

            if (!roles.Select(r => r.Value).Contains("SuperAdmin")) Result<bool>.Failure(Error.UnauthorizedError("You're unauthorized. 'SuperAdmin' role is not found."));

            return Result<bool>.Success(true);
        }

        private Result<bool> IsAuth()
        {
            var isAuth = _claimsPrincipal?.Identity?.IsAuthenticated ?? false;

            if (!isAuth) return Result<bool>.Failure(Error.UnauthorizedError());

            return Result<bool>.Success(isAuth);
        }
    }
}
