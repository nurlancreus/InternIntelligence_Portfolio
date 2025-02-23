using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail;
using InternIntelligence_Portfolio.Application.DTOs.Mail;
using InternIntelligence_Portfolio.Application.Options.Email;
using InternIntelligence_Portfolio.Domain.Abstractions;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace InternIntelligence_Portfolio.Infrastructure.Services.Mail
{
    public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
    {
        private readonly EmailSettings _emailConfig = emailSettings.Value;

        public async Task<Result<bool>> SendBulkEmailAsync(IEnumerable<RecipientDetailsDTO> recipientsDetails, string subject, string body, CancellationToken cancellationToken = default)
        {
            var message = new MessageDTO
            {
                Recipients = recipientsDetails.Select(r => new MailboxAddress(r.Name, r.Email)).ToList(),
                Subject = subject,
                Content = body
            };

            var emailMessage = CreateEmailMessage(message);
            var sendResult = await SendAsync(emailMessage, cancellationToken);

            if (sendResult.IsFailure) return Result<bool>.Failure(sendResult.Error);

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SendEmailAsync(RecipientDetailsDTO recipientDetails, string subject, string body, CancellationToken cancellationToken = default)
        {
            var message = new MessageDTO
            {
                To = new MailboxAddress(recipientDetails.Name, recipientDetails.Email),
                Subject = subject,
                Content = body
            };

            var emailMessage = CreateEmailMessage(message);
            var sendResult = await SendAsync(emailMessage, cancellationToken);

            if (sendResult.IsFailure) return Result<bool>.Failure(sendResult.Error);

            return Result<bool>.Success(true);
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

        private async Task<Result<bool>> SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken = default)
        {
            try
            {
                using var client = new SmtpClient();

                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls, cancellationToken);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password, cancellationToken);

                await client.SendAsync(mailMessage, cancellationToken);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(Error.UnexpectedError($"Unexpected Error happened while sending an email: {ex.Message}"));
            }
        }
    }
}
