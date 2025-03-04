using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Mail.Templates
{
    public interface IEmailTemplateService
    {
        string GenerateContactResponseTemplate(string recipientName, ContactSubject subject, string userMessage, string adminResponse);
        string GenerateContactReceivedTemplateForAdmin(string recipientName, ContactSubject subject);
        string GenerateContactRequestReceivedTemplate(string recipientName, ContactSubject subject);
    }
}
