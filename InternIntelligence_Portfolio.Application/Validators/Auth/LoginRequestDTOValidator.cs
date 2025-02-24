using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Auth;
using InternIntelligence_Portfolio.Domain;
using System.Text.RegularExpressions;

namespace InternIntelligence_Portfolio.Application.Validators.Auth
{
    public partial class LoginRequestDTOValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginRequestDTOValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                    .WithMessage("Username is required.")
                .MaximumLength(DomainConstants.User.UserNameMaxLength)
                    .WithMessage($"Username cannot exceed {DomainConstants.User.UserNameMaxLength} characters.");

            RuleFor(x => x.Password)
                 .NotEmpty()
                    .WithMessage("Password is required.")
                 .MinimumLength(12)
                    .WithMessage("Password must be at least 12 characters long.")
                 .MaximumLength(100)
                    .WithMessage("Password cannot exceed 100 characters.")
                 .Must(RequireDigit)
                    .WithMessage("Password must contain at least one digit.")
                 .Must(RequireLowercase)
                    .WithMessage("Password must contain at least one lowercase letter.")
                 .Must(RequireUppercase)
                    .WithMessage("Password must contain at least one uppercase letter.")
                 .Must(RequireNonAlphanumeric)
                    .WithMessage("Password must contain at least one non-alphanumeric character.")
                 .Must(RequireUniqueChars)
                    .WithMessage("Password must contain at least one unique character.");
        }

        private bool RequireDigit(string password)
        {
            return MyDigitRegex().IsMatch(password);
        }

        private bool RequireLowercase(string password)
        {
            return MyLowercaseRegex().IsMatch(password);
        }

        private bool RequireUppercase(string password)
        {
            return MyUppercaseRegex().IsMatch(password);
        }

        private bool RequireNonAlphanumeric(string password)
        {
            return MyNonAlphanumericRegex().IsMatch(password);
        }

        private bool RequireUniqueChars(string password)
        {
            return password.Distinct().Any();
        }

        [GeneratedRegex("[0-9]")]
        private static partial Regex MyDigitRegex();
        [GeneratedRegex("[a-z]")]
        private static partial Regex MyLowercaseRegex();
        [GeneratedRegex("[A-Z]")]
        private static partial Regex MyUppercaseRegex();
        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex MyNonAlphanumericRegex();
    }
}
