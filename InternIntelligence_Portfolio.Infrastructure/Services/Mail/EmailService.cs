using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail;
using InternIntelligence_Portfolio.Application.DTOs.Mail;
using InternIntelligence_Portfolio.Application.Options.Email;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Mail
{
    public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
    {
        private readonly EmailSettings _emailConfig = emailSettings.Value;

        public async Task SendBulkEmailAsync(IEnumerable<RecipientDetailsDTO> recipientsDetails, string subject, string body, CancellationToken cancellationToken = default)
        {
            var message = new MessageDTO
            {
                Recipients = recipientsDetails.Select(r => new MailboxAddress(r.Name, r.Email)).ToList(),
                Subject = subject,
                Content = body
            };

            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage, cancellationToken);
        }

        public async Task SendEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body, CancellationToken cancellationToken = default)
        {
            var message = new MessageDTO
            {
                To = new MailboxAddress(recipientDetails.Name, recipientDetails.Email),
                Subject = subject,
                Content = body
            };

            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage, cancellationToken);
        }

        private MimeMessage CreateEmailMessage(MessageDTO message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.UserName, _emailConfig.From));

            if (message.To != null)
            {
                emailMessage.To.Add(message.To);
            }
            else if (message.Recipients?.Count > 0)
            {
                emailMessage.To.AddRange(message.Recipients);
            }

            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken = default)
        {
            using var client = new SmtpClient();

            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            await client.SendAsync(mailMessage, cancellationToken);

        }
    }
}
