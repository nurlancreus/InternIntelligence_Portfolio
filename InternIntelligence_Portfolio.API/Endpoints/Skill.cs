using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Skill;
using Microsoft.AspNetCore.Mvc;

namespace InternIntelligence_Portfolio.API.Endpoints
{
    public static class Skill
    {
        public static IEndpointRouteBuilder RegisterSkillEndpoints(this IEndpointRouteBuilder routes)
        {
            var skill = routes.MapGroup("api/skills").RequireAuthorization(ApiConstants.AuthPolicies.OwnerPolicy);

            skill.MapGet("", async (ISkillService skillService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await skillService.GetAllAsync(cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            skill.MapGet("{id}", async ([FromRoute] Guid id, ISkillService skillService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await skillService.GetAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            skill.MapPost("", async (ISkillService skillService, HttpContext context, [FromBody] CreateSkillRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await skillService.CreateAsync(request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            skill.MapPatch("{id}", async ([FromRoute] Guid id, ISkillService skillService, HttpContext context, [FromBody] UpdateSkillRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await skillService.UpdateAsync(id, request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            skill.MapDelete("{id}", async ([FromRoute] Guid id, ISkillService skillService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await skillService.DeleteAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            return routes;
        }
    }
}
