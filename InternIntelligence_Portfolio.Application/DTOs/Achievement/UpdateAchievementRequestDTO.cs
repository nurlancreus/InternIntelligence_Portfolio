using InternIntelligence_Portfolio.Application.Abstractions;

namespace InternIntelligence_Portfolio.Application.DTOs.Achievement
{
    public record UpdateAchievementRequestDTO : IValidatableRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? AchievedAt { get; set; }
    }
}
