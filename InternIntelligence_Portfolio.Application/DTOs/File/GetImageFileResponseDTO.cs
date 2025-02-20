using InternIntelligence_Portfolio.Domain.Entities.Files;

namespace InternIntelligence_Portfolio.Application.DTOs.File
{
    public record GetImageFileResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GetImageFileResponseDTO(ApplicationFile applicationFile)
        {
            Id = applicationFile.Id;
            Name = applicationFile.Name;
            Path = applicationFile.Path;
            CreatedAt = applicationFile.CreatedAt;
            UpdatedAt = applicationFile.UpdatedAt;
        }
    }
}
