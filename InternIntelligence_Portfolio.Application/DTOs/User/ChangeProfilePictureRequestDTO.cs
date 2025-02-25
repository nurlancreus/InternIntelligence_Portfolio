using InternIntelligence_Portfolio.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternIntelligence_Portfolio.Application.DTOs.User
{
    public record ChangeProfilePictureRequestDTO : IValidatableRequest
    {
        [FromForm] public IFormFile NewProfilePictureFile { get; set; } = null!;
    }
}
