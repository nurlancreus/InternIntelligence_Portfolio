using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Identity;

namespace InternIntelligence_Portfolio.Domain.Entities
{
    public class Achievement : Base
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime AchievedAt { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        private Achievement() { }
        private Achievement(string title, string description, DateTime achievedAt)
        {
            Title = title;
            Description = description;
            AchievedAt = achievedAt;
        }

        public static Achievement Create(string title, string description, DateTime achievedAt)
        {
            return new Achievement(title, description, achievedAt);
        }
    }
}
