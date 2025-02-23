using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Auth;

namespace InternIntelligence_Portfolio.Application.Validators.Auth
{
    public class RefreshLoginRequestDTOValidator : AbstractValidator<RefreshLoginRequestDTO>
    {
        public RefreshLoginRequestDTOValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("AccessToken is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");
        }
    }
}
