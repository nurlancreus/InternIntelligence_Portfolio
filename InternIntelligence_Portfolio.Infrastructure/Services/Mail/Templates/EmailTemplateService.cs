using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail.Templates;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Mail.Templates
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateContactResponseTemplate(string recipientName, ContactSubject subject, string userMessage, string adminResponse)
        {
            return $@"
        <html>
        <body>
            <h2>Dear {recipientName},</h2>
            <p>Thank you for reaching out regarding <strong>{ContactEmailService.GetSubjectText(subject)}</strong>.</p>
            <p>Your original message:</p>
            <blockquote>{userMessage}</blockquote>

            <p><strong>Our Response:</strong></p>
            <blockquote>{adminResponse}</blockquote>

            <p>If you have any further questions, feel free to reach out!</p>
            <p>Best regards,<br/>The InternIntelligence Team</p>
        </body>
        </html>";
        }


        public string GenerateContactReceivedTemplateForAdmin(string recipientName, ContactSubject subject)
        {
            return $@"
                <html>
                <body>
                    <h2>New Contact Request</h2>
                    <p>You have received a new contact request from <strong>{recipientName}</strong>.</p>
                    <p><strong>Subject:</strong> {ContactEmailService.GetSubjectText(subject)}</p>
                    <p>Please review and respond accordingly.</p>
                    <p>Best regards,<br/>InternIntelligence System</p>
                </body>
                </html>";
        }

        public string GenerateContactRequestReceivedTemplate(string recipientName, ContactSubject subject)
        {
            return $@"
                <html>
                <body>
                    h2>Dear {recipientName},</h2>
                    <p>Thank you for reaching out regarding <strong>{ContactEmailService.GetSubjectText(subject)}</strong>. We have received your message and will get back to you as soon as possible.</p>
                    <p>Best regards,<br/>InternIntelligence System</p>
                </body>
                </html>";
        }
    }
}
