namespace InternIntelligence_Portfolio.Application.DTOs.Skill
{
    public record CreateSkillRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProficiencyLevel { get; set; } = string.Empty;
        public byte YearsOfExperience { get; set; }
    }
}
