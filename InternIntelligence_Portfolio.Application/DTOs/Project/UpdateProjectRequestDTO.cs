using Microsoft.AspNetCore.Http;

namespace InternIntelligence_Portfolio.Application.DTOs.Project
{
    public record UpdateProjectRequestDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public string? RepoUrl { get; set; }
        public string? LiveUrl { get; set; }
        public IFormFile? ProjectCoverImageFile { get; set; }
    }
}
