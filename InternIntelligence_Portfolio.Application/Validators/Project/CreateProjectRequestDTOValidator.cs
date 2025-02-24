using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Project;
using InternIntelligence_Portfolio.Application.Helpers;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.Project
{
    public class CreateProjectRequestDTOValidator : AbstractValidator<CreateProjectRequestDTO>
    {
        public CreateProjectRequestDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required.")
                .MaximumLength(DomainConstants.Project.NameMaxLength)
                    .WithMessage($"Name cannot exceed {DomainConstants.Project.NameMaxLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                    .WithMessage("Description is required.")
                .MaximumLength(DomainConstants.Project.DescriptionMaxLength)
                    .WithMessage($"Description cannot exceed {DomainConstants.Project.DescriptionMaxLength} characters.");

            RuleFor(x => x.RepoUrl)
                .NotEmpty()
                    .WithMessage("Repo url is required.")
                .Must(UrlValidator.IsUrlValid)
                    .WithMessage("Repo url is not valid.");

            RuleFor(x => x.LiveUrl)
                .NotEmpty()
                    .WithMessage("Live url is required.")
                .Must(UrlValidator.IsUrlValid)
                    .WithMessage("Live url is not valid.");

            RuleFor(x => x.ProjectCoverImageFile)
                .Must(FileHelpers.IsImage!)
                    .WithMessage("File should be image file.")
                .Must((file) => FileHelpers.IsSizeOk(file!, DomainConstants.Project.ProjectCoverImageMaxSizeInMb))
                    .WithMessage($"File size cannot exceed {DomainConstants.Project.ProjectCoverImageMaxSizeInMb} mb.")
                .When(x => x.ProjectCoverImageFile != null);
        }
    }
}
