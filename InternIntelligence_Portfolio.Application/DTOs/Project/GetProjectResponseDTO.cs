using ProjectEntity = InternIntelligence_Portfolio.Domain.Entities.Project;
using InternIntelligence_Portfolio.Application.DTOs.File;

namespace InternIntelligence_Portfolio.Application.DTOs.Project
{
    public record GetProjectResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RepoUrl { get; set; } = string.Empty;
        public string LiveUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public GetImageFileResponseDTO? ProjectCoverImageFile { get; set; }

        public GetProjectResponseDTO(ProjectEntity project)
        {
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            RepoUrl = project.RepoUrl;
            LiveUrl = project.LiveUrl;
            CreatedAt = project.CreatedAt;
            UpdatedAt = project.UpdatedAt;
            ProjectCoverImageFile = project.CoverImageFile is not null ? new GetImageFileResponseDTO(project.CoverImageFile) : null;

        }
    }
}
