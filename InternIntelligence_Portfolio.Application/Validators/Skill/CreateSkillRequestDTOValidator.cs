using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Domain.Enums;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.Skill
{
    public class CreateSkillRequestDTOValidator : AbstractValidator<CreateSkillRequestDTO>
    {
        public CreateSkillRequestDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required.")
                .MaximumLength(DomainConstants.Skill.NameMaxLength)
                    .WithMessage($"Name cannot exceed {DomainConstants.Skill.NameMaxLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                    .WithMessage("Description is required.")
                .MaximumLength(DomainConstants.Skill.DescriptionMaxLength)
                    .WithMessage($"Description cannot exceed {DomainConstants.Skill.DescriptionMaxLength} characters.");

            RuleFor(x => x.YearsOfExperience)
                .NotEmpty()
                    .WithMessage("YearsOfExperience is required.")
                .Must(years => years >= 0)
                    .WithMessage("Experience could not be negative.");

            RuleFor(x => x.ProficiencyLevel)
                .NotEmpty()
                    .WithMessage("ProficiencyLevel is required.")
                .Must(proficiency =>
                {
                    return Enum.TryParse<Proficiency>(proficiency, true, out var _);
                })
                    .WithMessage("ProficiencyLevel is not valid.");
        }
    }
}
