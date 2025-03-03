using AchievementEntity = InternIntelligence_Portfolio.Domain.Entities.Achievement;

namespace InternIntelligence_Portfolio.Application.DTOs.Achievement
{
    public record GetAchievementResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AchievedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GetAchievementResponseDTO() { }
        public GetAchievementResponseDTO(AchievementEntity achievement)
        {
            Id = achievement.Id;
            Title = achievement.Title;
            Description = achievement.Description;
            AchievedAt = achievement.AchievedAt;
            CreatedAt = achievement.CreatedAt;
            UpdatedAt = achievement.UpdatedAt;
        }
    }
}
