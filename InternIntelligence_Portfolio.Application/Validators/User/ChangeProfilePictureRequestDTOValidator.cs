using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.User;
using InternIntelligence_Portfolio.Application.Helpers;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.User
{
    public class ChangeProfilePictureRequestDTOValidator : AbstractValidator<ChangeProfilePictureRequestDTO>
    {
        public ChangeProfilePictureRequestDTOValidator()
        {
            RuleFor(x => x.NewProfilePictureFile)
                .NotEmpty()
                    .WithMessage("NewProfilePictureFile is required.")
                .Must(FileHelpers.IsImage)
                    .WithMessage("File should be image file.")
                .Must((file) => FileHelpers.IsSizeOk(file, DomainConstants.User.UserProfilePictureMaxSizeInMb))
                    .WithMessage($"File size cannot exceed {DomainConstants.User.UserProfilePictureMaxSizeInMb} mb.");
        }
    }
}
