using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Domain.Entities
{
    public class Skill : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Proficiency ProficiencyLevel { get; set; }
        public byte YearsOfExperience { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        private Skill() { }
        private Skill(string name, string description, Proficiency proficiencyLevel, byte yearsOfExperience)
        {
            Name = name;
            Description = description;
            ProficiencyLevel = proficiencyLevel;
            YearsOfExperience = yearsOfExperience;
        }

        public static Skill Create(string name, string description, Proficiency proficiencyLevel, byte yearsOfExperience)
        {
            return new Skill(name, description, proficiencyLevel, yearsOfExperience);
        }
    }
}
