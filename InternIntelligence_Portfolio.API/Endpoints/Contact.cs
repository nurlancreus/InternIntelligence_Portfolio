using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using Microsoft.AspNetCore.Mvc;

namespace InternIntelligence_Portfolio.API.Endpoints
{
    public static class Contact
    {
        public static IEndpointRouteBuilder RegisterContactEndpoints(this IEndpointRouteBuilder routes)
        {
            var contact = routes.MapGroup("api/contacts").RequireAuthorization(ApiConstants.AuthPolicies.OwnerPolicy);

            contact.MapGet("", async (IContactService contactService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await contactService.GetAllAsync(cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            contact.MapGet("{id}", async ([FromRoute] Guid id, IContactService contactService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await contactService.GetAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            contact.MapPost("", async (IContactService contactService, HttpContext context, [FromBody] CreateContactRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await contactService.CreateAsync(request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            }).AllowAnonymous();

            contact.MapPatch("{id}/answer", async ([FromRoute] Guid id, IContactService contactService, HttpContext context, [FromBody] AnswerContactRequestDTO request, CancellationToken cancellationToken = default) =>
            {
                var result = await contactService.AnswerAsync(id, request, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            contact.MapDelete("{id}", async ([FromRoute] Guid id, IContactService ContactService, HttpContext context, CancellationToken cancellationToken = default) =>
            {
                var result = await ContactService.DeleteAsync(id, cancellationToken);

                return result.Match(
                    (value) => Results.Ok(value),
                    (error) => error.HandleError(context));
            });

            return routes;
        }
    }

}
