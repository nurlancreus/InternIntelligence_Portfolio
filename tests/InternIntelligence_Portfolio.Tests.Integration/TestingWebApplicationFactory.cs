using InternIntelligence_Portfolio.API;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InternIntelligence_Portfolio.Tests.Integration
{
    public class TestingWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing"); // Ensure tests use the "Testing" environment

            // Manually load User Secrets for the Testing project
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets<TestingWebApplicationFactory>() // Load User Secrets of the Testing project
                    .AddEnvironmentVariables();
            });

            builder.ConfigureServices((context, services) =>
            {
                var config = context.Configuration;
                var connectionString = config["ConnectionStrings:Default"] ?? "UseInMemoryDB"; // Ensure DB is from User Secrets

                // Remove existing DbContext registrations
                var contextOptionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (contextOptionsDescriptor != null)
                {
                    services.Remove(contextOptionsDescriptor);
                }

                var contextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(AppDbContext));

                if (contextDescriptor != null)
                {
                    services.Remove(contextDescriptor);
                }

                // Remove all registered database providers
                var dbContextDescriptors = services
                    .Where(d => d.ServiceType == typeof(DbContextOptions))
                    .ToList();

                foreach (var descriptor in dbContextDescriptors)
                {
                    services.Remove(descriptor);
                }

                // Register In-Memory Database
                services.AddDbContext<AppDbContext>((sp, options) =>
                {
                    options.UseInMemoryDatabase(connectionString);
                    options.AddInterceptors(sp.GetRequiredService<CustomSaveChangesInterceptor>());
                });
            });
        }
    }
}
