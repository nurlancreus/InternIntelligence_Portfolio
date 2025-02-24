using InternIntelligence_Portfolio.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace InternIntelligence_Portfolio.Application.DTOs.User
{
    public record ChangeProfilePictureRequestDTO : IValidatableRequest
    {
        public IFormFile NewProfilePictureFile { get; set; } = null!;
    }
}
