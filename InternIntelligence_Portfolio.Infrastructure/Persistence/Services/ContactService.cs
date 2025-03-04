using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.Abstractions.Repositories;
using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities;
using InternIntelligence_Portfolio.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Services
{
    public class ContactService(IJwtSession jwtSession, IUnitOfWork unitOfWork, IContactEmailService contactEmailService) : IContactService
    {
        private readonly IJwtSession _jwtSession = jwtSession;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRepository<Contact> _contactRepository = unitOfWork.GetRepository<Contact>();
        private readonly IContactEmailService _contactEmailService = contactEmailService;

        public async Task<Result<bool>> AnswerAsync(Guid id, AnswerContactRequestDTO answerContactRequest, CancellationToken cancellationToken = default)
        {
            var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var isSuperAdminResult = _jwtSession.ValidateIfSuperAdmin();
            if (isSuperAdminResult.IsFailure) return Result<bool>.Failure(isSuperAdminResult.Error);

            var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

            if (contact is null)
                return Result<bool>.Failure(Error.NotFoundError("Contact is not found."));

            var isContactResponseMessageSendResult = await _contactEmailService.SendContactResponseMessageAsync(contact.FirstName, contact.LastName, contact.Email, contact.Subject, contact.Message, answerContactRequest.Message, cancellationToken);

            if (isContactResponseMessageSendResult.IsFailure)
                return Result<bool>.Failure(isContactResponseMessageSendResult.Error);

            contact.IsAnswered = true;

            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if(!isSaved) 
                return Result<bool>.Failure(Error.UnexpectedError("Contact could not be updated."));

            scope.Complete();

            return Result<bool>.Success(true);
        }

        public async Task<Result<Guid>> CreateAsync(CreateContactRequestDTO createContactRequest, CancellationToken cancellationToken = default)
        {
            var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!Enum.TryParse<ContactSubject>(createContactRequest.Subject, true, out var contactSubject))
                return Result<Guid>.Failure(Error.UnexpectedError("Could not parse contact subject enum."));

            var contact = Contact.Create(createContactRequest.FirstName, createContactRequest.LastName, createContactRequest.Email, createContactRequest.Message, contactSubject);

            var isAdded = await _contactRepository.AddAsync(contact, cancellationToken);

            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isAdded || !isSaved)
                return Result<Guid>.Failure(Error.UnexpectedError("New contact could not be created."));

            var isContactReceiveMessageSentResult = await _contactEmailService.SendContactReceivedMessageAsync(contact.FirstName, contact.LastName, contact.Email, contact.Subject, cancellationToken);

            if (isContactReceiveMessageSentResult.IsFailure)
                return Result<Guid>.Failure(isContactReceiveMessageSentResult.Error);

            scope.Complete();

            return Result<Guid>.Success(contact.Id);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var contact = await _contactRepository.GetByIdAsync(id, cancellationToken);

            if (contact is null) return Result<bool>.Failure(Error.NotFoundError("Contact is not found."));

            var isDeleted = _contactRepository.Delete(contact);

            var isSaved = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!isDeleted || !isSaved) return Result<bool>.Failure(Error.UnexpectedError("Contact could not be deleted."));

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<GetContactResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var contacts = await _contactRepository.GetAll(false).Select(c => new GetContactResponseDTO(c)).ToListAsync(cancellationToken);

            return Result<IEnumerable<GetContactResponseDTO>>.Success(contacts);
        }

        public async Task<Result<GetContactResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var contact = await _contactRepository.GetByIdAsync(id, cancellationToken, false);

            if (contact is null) return Result<GetContactResponseDTO>.Failure(Error.NotFoundError("Contact is not found."));

            return Result<GetContactResponseDTO>.Success(new GetContactResponseDTO(contact));
        }
    }
}
