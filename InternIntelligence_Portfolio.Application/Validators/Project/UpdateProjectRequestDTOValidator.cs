using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Project;
using InternIntelligence_Portfolio.Application.Helpers;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.Project
{
    public class UpdateProjectRequestDTOValidator : AbstractValidator<UpdateProjectRequestDTO>
    {
        public UpdateProjectRequestDTOValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(DomainConstants.Project.NameMaxLength)
                    .WithMessage($"Name cannot exceed {DomainConstants.Project.NameMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Description)
                .MaximumLength(DomainConstants.Project.DescriptionMaxLength)
                    .WithMessage($"Description cannot exceed {DomainConstants.Project.DescriptionMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
            
            RuleFor(x => x.RepoUrl)
                .Must(UrlValidator.IsUrlValid!)
                    .WithMessage("Repo url is not valid.")
                .When(x => !string.IsNullOrEmpty(x.RepoUrl));

            RuleFor(x => x.LiveUrl)
                .Must(UrlValidator.IsUrlValid!)
                    .WithMessage("Live url is not valid.")
                .When(x => !string.IsNullOrEmpty(x.LiveUrl));

            RuleFor(x => x.ProjectCoverImageFile)
                .Must(FileHelpers.IsImage!)
                    .WithMessage("File should be image file.")
                .Must((file) => FileHelpers.IsSizeOk(file!, DomainConstants.Project.ProjectCoverImageMaxSizeInMb))
                    .WithMessage($"File size cannot exceed {DomainConstants.Project.ProjectCoverImageMaxSizeInMb} mb.")
                .When(x => x.ProjectCoverImageFile != null);
        }
    }
}
