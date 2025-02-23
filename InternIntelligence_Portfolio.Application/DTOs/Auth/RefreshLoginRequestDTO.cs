using InternIntelligence_Portfolio.Application.Abstractions;

namespace InternIntelligence_Portfolio.Application.DTOs.Auth
{
    public record RefreshLoginRequestDTO : IValidatableRequest
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
