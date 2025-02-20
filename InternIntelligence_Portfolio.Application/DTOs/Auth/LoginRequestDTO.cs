namespace InternIntelligence_Portfolio.Application.DTOs.Auth
{
    public record LoginRequestDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
