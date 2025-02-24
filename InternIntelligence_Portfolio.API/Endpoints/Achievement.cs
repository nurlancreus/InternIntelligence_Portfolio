using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using Microsoft.AspNetCore.Mvc;

namespace InternIntelligence_Portfolio.API.Endpoints
{
    public static class Achievement
    {
        public static IEndpointRouteBuilder RegisterAchievementEndpoints(this IEndpointRouteBuilder routes)
        {
            var achievement = routes.MapGroup("api/achievements").RequireAuthorization(ApiConstants.AuthPolicies.OwnerPolicy);

            achievement.MapGet("", async (IAchievementService achievementService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await achievementService.GetAllAsync(cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            achievement.MapGet("{id}", async ([FromRoute] Guid id, IAchievementService achievementService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await achievementService.GetAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            achievement.MapPost("", async (IAchievementService achievementService, HttpContext context, [FromBody] CreateAchievementRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await achievementService.CreateAsync(request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            achievement.MapPatch("{id}", async ([FromRoute] Guid id, IAchievementService achievementService, HttpContext context, [FromBody] UpdateAchievementRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await achievementService.UpdateAsync(id, request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            achievement.MapDelete("{id}", async ([FromRoute] Guid id, IAchievementService achievementService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await achievementService.DeleteAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            return routes;
        }
    }

}
