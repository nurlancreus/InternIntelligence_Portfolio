using InternIntelligence_Portfolio.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace InternIntelligence_Portfolio.Application.DTOs.Project
{
    public record UpdateProjectRequestDTO : IValidatableRequest
    {
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public string? RepoUrl { get; set; }
        public string? LiveUrl { get; set; }
        public IFormFile? ProjectCoverImageFile { get; set; }
    }
}
