using InternIntelligence_Portfolio.Application.Abstractions;

namespace InternIntelligence_Portfolio.Application.DTOs.Auth
{
    public record LoginRequestDTO : IValidatableRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
