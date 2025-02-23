
using InternIntelligence_Portfolio.API.Endpoints;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;

namespace InternIntelligence_Portfolio.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.RegisterServices();

            var app = builder.Build();

            app.AddMiddlewares();

            #region Seed Super Admin
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DbInitializer.SeedSuperAdmin(services);
            }
            #endregion

            app.RegisterAuthEndpoints();

            app.Run();
        }
    }
}
