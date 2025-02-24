using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Application.Validators.Contact
{
    public class CreateContactRequestDTOValidator : AbstractValidator<CreateContactRequestDTO>
    {
        public CreateContactRequestDTOValidator()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty()
                    .WithMessage("Firstname is required.")
                .MaximumLength(DomainConstants.Contact.FirstNameMaxLength)
                    .WithMessage($"Firstname cannot exceed {DomainConstants.Contact.FirstNameMaxLength} characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                    .WithMessage("Lastname is required.")
                .MaximumLength(DomainConstants.Contact.LastNameMaxLength)
                    .WithMessage($"Lastname cannot exceed {DomainConstants.Contact.LastNameMaxLength} characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Email address is required.")
                .MaximumLength(DomainConstants.Contact.EmailMaxLength)
                    .WithMessage($"Email address cannot exceed {DomainConstants.Contact.EmailMaxLength} characters.")
                .EmailAddress()
                    .WithMessage("Email address is not valid");

            RuleFor(x => x.Message)
                .NotEmpty()
                    .WithMessage("Message is required.")
                .MaximumLength(DomainConstants.Contact.MessageMaxLength)
                    .WithMessage($"Message cannot exceed {DomainConstants.Contact.MessageMaxLength} characters.");

            RuleFor(x => x.Subject)
                .NotEmpty()
                    .WithMessage("Subject is required.")
                .Must(subject =>
                {
                    return Enum.TryParse<ContactSubject>(subject, true, out var _);
                })
                    .WithMessage("Subject is not valid.");
        }
    }
}
