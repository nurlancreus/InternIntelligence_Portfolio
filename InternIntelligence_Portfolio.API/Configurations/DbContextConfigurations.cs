using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace InternIntelligence_Portfolio.API.Configurations
{
    public static class DbContextConfigurations
    {
        public static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder builder)
        {
            // Add DbContext Interceptors 
            builder.Services.AddScoped<CustomSaveChangesInterceptor>();

            if (!builder.Environment.IsTesting())
            {
                builder.Services.AddDbContext<AppDbContext>((sp, options) =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlOptions => sqlOptions
                    .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);

                    options.AddInterceptors(sp.GetRequiredService<CustomSaveChangesInterceptor>());
                });
            }

            return builder;
        }
    }
}
