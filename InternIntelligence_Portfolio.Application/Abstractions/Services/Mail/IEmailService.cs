using InternIntelligence_Portfolio.Application.DTOs.Mail;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Mail
{
    public interface IEmailService
    {
        Task SendBulkEmailAsync(IEnumerable<RecipientDetailsDTO> recipientsDetails, string subject, string body, CancellationToken cancellationToken = default);
        Task SendEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body, CancellationToken cancellationToken = default);
    }
}
