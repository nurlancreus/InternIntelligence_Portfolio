using InternIntelligence_Portfolio.API;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InternIntelligence_Portfolio.Tests.Integration
{
    public class TestingWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing"); // Ensure tests use Test config

            builder.ConfigureServices(services =>
            {
                // Remove any existing DbContextOptions registrations
                var contextOptionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (contextOptionsDescriptor != null)
                {
                    services.Remove(contextOptionsDescriptor);
                }

                // Remove any existing AppDbContext registrations
                var contextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(AppDbContext));

                if (contextDescriptor != null)
                {
                    services.Remove(contextDescriptor);
                }

                // Remove ALL registered database providers
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
                    options.UseInMemoryDatabase("UseInMemoryDatabase");

                    options.AddInterceptors(sp.GetRequiredService<CustomSaveChangesInterceptor>());
                });
            });
        }
    }
}
