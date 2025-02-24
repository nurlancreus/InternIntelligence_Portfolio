using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services
{
    public interface IContactService
    {
        Task<Result<IEnumerable<GetContactResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<GetContactResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Guid>> CreateAsync(CreateContactRequestDTO createContactRequest, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<bool>> AnswerAsync(Guid id, AnswerContactRequestDTO answerContactRequest, CancellationToken cancellationToken = default);
    }
}
