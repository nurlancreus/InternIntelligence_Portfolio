using FluentValidation;
using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.Abstractions.Repositories;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Domain;
using ContactEntity = InternIntelligence_Portfolio.Domain.Entities.Contact;

namespace InternIntelligence_Portfolio.Application.Validators.Contact
{
    public class AnswerContactRequestDTOValidator : AbstractValidator<AnswerContactRequestDTO>
    {
        public AnswerContactRequestDTOValidator()
        {

            RuleFor(x => x.Message)
                .NotEmpty()
                    .WithMessage("Message is required.")
                .MaximumLength(DomainConstants.Contact.MessageMaxLength)
                    .WithMessage($"Message cannot exceed {DomainConstants.Contact.MessageMaxLength} characters.");
        }
    }
}
