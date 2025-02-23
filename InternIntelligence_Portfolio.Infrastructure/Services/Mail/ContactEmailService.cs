using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail.Templates;
using InternIntelligence_Portfolio.Application.DTOs.Mail;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Mail
{
    public class ContactEmailService(IEmailService emailService, IEmailTemplateService emailTemplateService) : IContactEmailService
    {
        private readonly IEmailService _emailService = emailService;
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;

        public async Task<Result<bool>> SendContactReceivedMessageAsync(string firstName, string lastName, string email, ContactSubject subject, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(firstName, lastName);

            var body = _emailTemplateService.GenerateContactReceivedTemplate(recipientName, subject);

            var recipient = new RecipientDetailsDTO
            {
                Email = email,
                Name = recipientName,
            };

            var sendEmailResult = await _emailService.SendEmailAsync(recipient, GetSubjectText(subject), body, cancellationToken);

            if (sendEmailResult.IsFailure) return Result<bool>.Failure(sendEmailResult.Error);

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SendContactResponseMessageAsync(string firstName, string lastName, string email, ContactSubject subject, string message, CancellationToken cancellationToken = default)
        {
            var recipientName = GetFullName(firstName, lastName);

            var body = _emailTemplateService.GenerateContactResponseTemplate(recipientName, subject, message);

            var recipient = new RecipientDetailsDTO
            {
                Email = email,
                Name = recipientName,
            };

            var sendEmailResult = await _emailService.SendEmailAsync(recipient, GetSubjectText(subject), body, cancellationToken);

            if (sendEmailResult.IsFailure) return Result<bool>.Failure(sendEmailResult.Error);

            return Result<bool>.Success(true);
        }

        private static string GetFullName(string firstName, string lastName) => $"{firstName} {lastName}";

        public static string GetSubjectText(ContactSubject contactSubject)
        {
            return contactSubject.ToString().Replace('_', ' ');
        }
    }
}
