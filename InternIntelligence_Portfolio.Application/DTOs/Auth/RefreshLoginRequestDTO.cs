namespace InternIntelligence_Portfolio.Application.DTOs.Auth
{
    public record RefreshLoginRequestDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
