using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace InternIntelligence_Portfolio.API.Endpoints
{
    public static class User
    {
        public static IEndpointRouteBuilder RegisterUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var user = routes.MapGroup("api/users").RequireAuthorization(ApiConstants.AuthPolicies.OwnerPolicy);

            user.MapGet("me", async (IUserService userService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await userService.GetMeAsync(cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            user.MapPatch("change-picture", async (IUserService userService, HttpContext context, [FromForm] ChangeProfilePictureRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await userService.ChangeProfilePictureAsync(request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            return routes;
        }
    }

}
