using Microsoft.AspNetCore.Http;

namespace InternIntelligence_Portfolio.Application.DTOs.Project
{
    public record CreateProjectRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RepoUrl { get; set; } = string.Empty;
        public string LiveUrl { get; set; } = string.Empty;
        public IFormFile? ProjectCoverImageFile { get; set; }
    }
}
