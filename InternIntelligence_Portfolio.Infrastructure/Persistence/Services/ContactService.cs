using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Services
{
    public class ContactService : IContactService
    {
        public Task<Result<bool>> AnswerAsync(AnswerContactRequestDTO answerContactRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Guid>> CreateAsync(CreateContactRequestDTO createContactRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<GetContactResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<GetContactResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
