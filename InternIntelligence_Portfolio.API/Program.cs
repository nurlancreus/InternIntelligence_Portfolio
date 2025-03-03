
using InternIntelligence_Portfolio.API.Configurations;
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

            await app.AddMiddlewaresAsync();

            app.RegisterAuthEndpoints()
               .RegisterUserEndpoints()
               .RegisterProjectEndpoints()
               .RegisterAchievementEndpoints()
               .RegisterSkillEndpoints()
               .RegisterContactEndpoints();
               

            app.Run();
        }
    }
}
