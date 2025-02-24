using InternIntelligence_Portfolio.Application.Abstractions;

namespace InternIntelligence_Portfolio.Application.DTOs.Skill
{
    public record UpdateSkillRequestDTO : IValidatableRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public string? ProficiencyLevel { get; set; } 
        public byte? YearsOfExperience { get; set; }
    }
}
