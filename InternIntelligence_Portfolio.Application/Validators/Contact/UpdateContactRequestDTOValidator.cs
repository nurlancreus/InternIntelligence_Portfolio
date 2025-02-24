using FluentValidation;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Domain.Enums;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Application.Validators.Contact
{
    public class UpdateContactRequestDTOValidator : AbstractValidator<UpdateContactRequestDTO>
    {
        public UpdateContactRequestDTOValidator()
        {
            RuleFor(x => x.FirstName)
               .MaximumLength(DomainConstants.Contact.FirstNameMaxLength)
                   .WithMessage($"Firstname cannot exceed {DomainConstants.Contact.FirstNameMaxLength} characters.")
               .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(DomainConstants.Contact.LastNameMaxLength)
                    .WithMessage($"Lastname cannot exceed {DomainConstants.Contact.LastNameMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Email)
                .MaximumLength(DomainConstants.Contact.EmailMaxLength)
                    .WithMessage($"Email address cannot exceed {DomainConstants.Contact.EmailMaxLength} characters.")
                .EmailAddress()
                    .WithMessage("Email address is not valid")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Message)
                .MaximumLength(DomainConstants.Contact.MessageMaxLength)
                    .WithMessage($"Message cannot exceed {DomainConstants.Contact.MessageMaxLength} characters.")
                .When(x => !string.IsNullOrEmpty(x.Message));

            RuleFor(x => x.Subject)
                .Must(subject =>
                {
                    return Enum.TryParse<ContactSubject>(subject, true, out var _);
                })
                    .WithMessage("Subject is not valid.")
                .When(x => !string.IsNullOrEmpty(x.Subject));
        }
    }
}
