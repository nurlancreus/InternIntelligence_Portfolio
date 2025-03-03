using FluentValidation;
using FluentValidation.AspNetCore;
using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Application.Validators.Auth;
using InternIntelligence_Portfolio.Infrastructure.Persistence;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;

namespace InternIntelligence_Portfolio.API.Configurations
{
    public static class Configurations
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            #region Configure Environments
            // Load configurations based on environment
            builder.ConfigureEnvironments();

            #endregion

            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region Register CORS
            builder.ConfigureCORS();
            #endregion

            #region Register Rate Limiter
            builder.ConfigureRateLimiter();

            #endregion

            #region Register Identity
            builder.ConfigureIdentity();
            #endregion

            #region Register Auth
            builder.ConfigureAuth();
            #endregion

            #region Register DbContext
            builder.ConfigureDbContext();

            #endregion

            #region Register Fluent Validation
            builder.Services
             .AddFluentValidationClientsideAdapters()
             .AddValidatorsFromAssemblyContaining<LoginRequestDTOValidator>();

            builder.Services.AddSingleton<RequestValidator>();
            #endregion

            #region Register App Interfaces
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion;

            #region Register App Services
            builder.ConfigureServices();
            #endregion

            #region Exception Handling
            // Add ProblemDetails services
            builder.Services.AddProblemDetails();

            // Add Exception Handler
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            #endregion

            #region Register Options
            builder.ConfigureOptions();
            #endregion
        }

        public static async Task AddMiddlewaresAsync(this WebApplication app)
        {
            app.UseExceptionHandler();

            app.UseStatusCodePages();

            app.UseCors(ApiConstants.CorsPolicies.AllowAllPolicy);

            app.UseRateLimiter();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            #region Seed Super Admin
            if (!app.Environment.IsTesting())
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                await DbInitializer.SeedSuperAdmin(services);
            }
            #endregion
        }
    }
}
