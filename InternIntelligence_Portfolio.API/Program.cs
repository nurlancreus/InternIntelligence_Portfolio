
using InternIntelligence_Portfolio.API.Endpoints;

namespace InternIntelligence_Portfolio.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.RegisterServices();

            var app = builder.Build();

            app.AddMiddlewares();

            app.RegisterAuthEndpoints();

            app.Run();
        }
    }
}
