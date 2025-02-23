using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Mail.Templates
{
    public interface IEmailTemplateService
    {
        string GenerateContactResponseTemplate(string recipientName, ContactSubject subject, string message);
        string GenerateContactReceivedTemplate(string recipientName, ContactSubject subject);
    }
}
