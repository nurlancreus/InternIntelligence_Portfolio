using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Project;
using Microsoft.AspNetCore.Mvc;

namespace InternIntelligence_Portfolio.API.Endpoints
{
    public static class Project
    {
        public static IEndpointRouteBuilder RegisterProjectEndpoints(this IEndpointRouteBuilder routes)
        {
            var project = routes.MapGroup("api/projects").RequireAuthorization(ApiConstants.AuthPolicies.OwnerPolicy).DisableAntiforgery();

            project.MapGet("", async (IProjectService projectService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await projectService.GetAllAsync(cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            project.MapGet("{id}", async ([FromRoute] Guid id, IProjectService projectService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await projectService.GetAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            project.MapPost("", async (IProjectService projectService, HttpContext context, [FromForm] CreateProjectRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await projectService.CreateAsync(request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            project.MapPatch("{id}", async ([FromRoute] Guid id, IProjectService projectService, HttpContext context, [FromForm] UpdateProjectRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await projectService.UpdateAsync(id, request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            project.MapDelete("{id}", async ([FromRoute] Guid id, IProjectService projectService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await projectService.DeleteAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            return routes;
        }
    }

}
