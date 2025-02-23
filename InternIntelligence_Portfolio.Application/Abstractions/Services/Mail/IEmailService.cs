using InternIntelligence_Portfolio.Application.DTOs.Mail;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Mail
{
    public interface IEmailService
    {
        Task<Result<bool>> SendBulkEmailAsync(IEnumerable<RecipientDetailsDTO> recipientsDetails, string subject, string body, CancellationToken cancellationToken = default);
        Task<Result<bool>> SendEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body, CancellationToken cancellationToken = default);
    }
}
