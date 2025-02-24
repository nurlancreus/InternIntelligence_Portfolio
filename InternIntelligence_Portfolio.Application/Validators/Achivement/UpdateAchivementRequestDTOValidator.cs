using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.Achivement
{
    public class UpdateAchivementRequestDTOValidator : AbstractValidator<UpdateAchievementRequestDTO>
    {
        public UpdateAchivementRequestDTOValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(DomainConstants.Achievement.TitleMaxLength)
                    .WithMessage($"Title cannot exceed {DomainConstants.Achievement.TitleMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Description)
                .MaximumLength(DomainConstants.Achievement.DescriptionMaxLength)
                    .WithMessage($"Description cannot exceed {DomainConstants.Achievement.DescriptionMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.AchievedAt)
                .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Date should not be in future.")
                .When(x => x.AchievedAt != null);
        }
    }
}
