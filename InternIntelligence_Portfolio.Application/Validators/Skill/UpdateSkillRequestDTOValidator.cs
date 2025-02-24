using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Domain.Enums;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.Skill
{
    public class UpdateSkillRequestDTOValidator : AbstractValidator<UpdateSkillRequestDTO>
    {
        public UpdateSkillRequestDTOValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(DomainConstants.Skill.NameMaxLength)
                    .WithMessage($"Name cannot exceed {DomainConstants.Skill.NameMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));


            RuleFor(x => x.Description)
                .MaximumLength(DomainConstants.Skill.DescriptionMaxLength)
                    .WithMessage($"Description cannot exceed {DomainConstants.Skill.DescriptionMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.ProficiencyLevel)
                .Must(proficiency =>
                {
                    return Enum.TryParse<Proficiency>(proficiency, true, out var _);
                })
                    .WithMessage("ProficiencyLevel is not valid.")
                .When(x => x.ProficiencyLevel != null);
        }
    }
}
