using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Decorators
{
    public class ContactServiceValidationDecorator(IContactService innerContactService, RequestValidator requestValidator) : IContactService
    {
        private readonly IContactService _innerContactService = innerContactService;
        private readonly RequestValidator _requestValidator = requestValidator;

        public async Task<Result<bool>> AnswerAsync(AnswerContactRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsFailure)
                return Result<bool>.Failure(validationResult.Error);

            return await _innerContactService.AnswerAsync(request, cancellationToken);
        }

        public async Task<Result<Guid>> CreateAsync(CreateContactRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsFailure)
                return Result<Guid>.Failure(validationResult.Error);

            return await _innerContactService.CreateAsync(request, cancellationToken);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _innerContactService.DeleteAsync(id, cancellationToken);
        }

        public async Task<Result<IEnumerable<GetContactResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _innerContactService.GetAllAsync(cancellationToken);
        }

        public async Task<Result<GetContactResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _innerContactService.GetAsync(id, cancellationToken);
        }
    }
}
