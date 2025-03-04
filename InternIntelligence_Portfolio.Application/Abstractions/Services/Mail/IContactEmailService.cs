using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Mail
{
    public interface IContactEmailService
    {
        Task<Result<bool>> SendContactReceivedMessageAsync(string firstName, string lastName, string email, ContactSubject subject, CancellationToken cancellationToken = default);
        Task<Result<bool>> SendContactResponseMessageAsync(string firstName, string lastName, string email, ContactSubject subject, string userMessage, string adminResponse, CancellationToken cancellationToken = default);
    }
}
