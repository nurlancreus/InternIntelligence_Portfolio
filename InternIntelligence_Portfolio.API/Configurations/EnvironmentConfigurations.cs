namespace InternIntelligence_Portfolio.API.Configurations
{
    public static class EnvironmentConfigurations
    {
        public static WebApplicationBuilder ConfigureEnvironments(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(); // Load User Secrets in ALL environments

            return builder;
        }
    }
}
