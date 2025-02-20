namespace InternIntelligence_Portfolio.Application.DTOs.Achievement
{
    public record UpdateAchievementRequestDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? AchievedAt { get; set; }
    }
}
