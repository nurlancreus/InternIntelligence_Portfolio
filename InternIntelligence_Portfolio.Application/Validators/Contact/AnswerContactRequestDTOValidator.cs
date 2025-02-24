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
        private readonly IRepository<ContactEntity> _contactRepository;
        public AnswerContactRequestDTOValidator(IUnitOfWork unitOfWork)
        {
            _contactRepository = unitOfWork.GetRepository<ContactEntity>();

            RuleFor(x => x.ContactId)
                .NotEmpty()
                    .WithMessage("Contact id is required.")
                .MustAsync(async (id, cancellationToken) =>
                {
                    var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

                    return contact != null;
                })
                    .WithMessage("Contact is not found.");

            RuleFor(x => x.Message)
                .NotEmpty()
                    .WithMessage("Message is required.")
                .MaximumLength(DomainConstants.Contact.MessageMaxLength)
                    .WithMessage($"Message cannot exceed {DomainConstants.Contact.MessageMaxLength} characters.");
        }
    }
}
