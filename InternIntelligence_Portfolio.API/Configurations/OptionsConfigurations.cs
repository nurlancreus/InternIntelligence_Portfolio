using InternIntelligence_Portfolio.Application.Options.Email;
using InternIntelligence_Portfolio.Application.Options.Token;

namespace InternIntelligence_Portfolio.API.Configurations
{
    public static class OptionsConfigurations
    {
        public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("Token"));

            return builder;
        }
    }
}
