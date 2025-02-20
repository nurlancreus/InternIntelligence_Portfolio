using Microsoft.AspNetCore.Http;

namespace InternIntelligence_Portfolio.Application.DTOs.User
{
    public record ChangeProfilePictureRequestDTO
    {
        public IFormFile NewProfilePictureFile { get; set; } = null!;
    }
}
