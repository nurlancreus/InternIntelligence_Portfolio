using SkillEntity = InternIntelligence_Portfolio.Domain.Entities.Skill;

namespace InternIntelligence_Portfolio.Application.DTOs.Skill
{
    public record GetSkillResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProficiencyLevel { get; set; }
        public byte YearsOfExperience { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GetSkillResponseDTO(SkillEntity skill)
        {
            Id = skill.Id;
            Name = skill.Name;
            Description = skill.Description;
            ProficiencyLevel = skill.ProficiencyLevel.ToString();
            YearsOfExperience = skill.YearsOfExperience;
            CreatedAt = skill.CreatedAt;
            UpdatedAt = skill.UpdatedAt;
        }
    }
}
