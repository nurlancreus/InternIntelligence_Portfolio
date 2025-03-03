namespace InternIntelligence_Portfolio.API.Configurations
{
    public static class CorsConfigurations
    {
        public static WebApplicationBuilder ConfigureCORS(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(ApiConstants.CorsPolicies.AllowAllPolicy, builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });

            return builder;
        }
    }
}
