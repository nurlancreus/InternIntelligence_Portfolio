namespace InternIntelligence_Portfolio.Application.DTOs.Achievement
{
    public record CreateAchievementRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AchievedAt { get; set; }
    }
}
