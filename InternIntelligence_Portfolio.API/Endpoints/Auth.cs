using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace InternIntelligence_Portfolio.API.Endpoints
{
    public static class Auth
    {
        public static IEndpointRouteBuilder RegisterAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var auth = routes.MapGroup("api/auth").AllowAnonymous();

            auth.MapPost("login", async (IAuthService authService, HttpContext context, [FromBody] LoginRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await authService.LoginAsync(request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            auth.MapPost("refresh-login", async (IAuthService authService, HttpContext context, [FromBody] RefreshLoginRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await authService.RefreshLoginAsync(request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            return routes;
        }
    }
}
