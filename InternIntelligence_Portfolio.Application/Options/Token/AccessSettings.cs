namespace InternIntelligence_Portfolio.Application.Options.Token
{
    public class AccessSettings
    {
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string SecurityKey { get; set; } = string.Empty;
        public int AccessTokenLifeTimeInMinutes { get; set; }
    }
}
