using InternIntelligence_Portfolio.Application.Abstractions;

namespace InternIntelligence_Portfolio.Application.DTOs.Contact
{
    public record AnswerContactRequestDTO : IValidatableRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}
