using InternIntelligence_Portfolio.Application.Abstractions;

namespace InternIntelligence_Portfolio.Application.DTOs.Contact
{
    public record CreateContactRequestDTO : IValidatableRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}
