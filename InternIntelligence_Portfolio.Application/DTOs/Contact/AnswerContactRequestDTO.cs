using InternIntelligence_Portfolio.Application.Abstractions;

namespace InternIntelligence_Portfolio.Application.DTOs.Contact
{
    public record AnswerContactRequestDTO : IValidatableRequest
    {
        public Guid ContactId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
