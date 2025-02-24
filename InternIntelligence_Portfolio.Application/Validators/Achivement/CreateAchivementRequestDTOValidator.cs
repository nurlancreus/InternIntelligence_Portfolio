using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.Achivement
{
    public class CreateAchivementRequestDTOValidator : AbstractValidator<CreateAchievementRequestDTO>
    {
        public CreateAchivementRequestDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                    .WithMessage("Achievement title is required.")
                .MaximumLength(DomainConstants.Achievement.TitleMaxLength)
                    .WithMessage($"Title cannot exceed {DomainConstants.Achievement.TitleMaxLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                    .WithMessage("Achievement decription is required.")
                .MaximumLength(DomainConstants.Achievement.DescriptionMaxLength)
                    .WithMessage($"Description cannot exceed {DomainConstants.Achievement.DescriptionMaxLength} characters.");

            RuleFor(x => x.AchievedAt)
                .NotEmpty()
                    .WithMessage("Achievement date is required.")
                .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Date should not be in future.");

        }
    }
}
