using FluentValidation.AspNetCore;
using FluentValidation;
using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Storage;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Token;
using InternIntelligence_Portfolio.Application.Options.Email;
using InternIntelligence_Portfolio.Application.Options.Token;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using InternIntelligence_Portfolio.Infrastructure.Persistence;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Services;
using InternIntelligence_Portfolio.Infrastructure.Services.Mail;
using InternIntelligence_Portfolio.Infrastructure.Services.Sessions;
using InternIntelligence_Portfolio.Infrastructure.Services.Storage;
using InternIntelligence_Portfolio.Infrastructure.Services.Storage.Local;
using InternIntelligence_Portfolio.Infrastructure.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.RateLimiting;
using InternIntelligence_Portfolio.Application.Validators.Auth;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Application.Decorators;
using InternIntelligence_Portfolio.Infrastructure.Services.Mail.Templates;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail.Templates;

namespace InternIntelligence_Portfolio.API
{
    public static class Configurations
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            #region Configure Environments
            // Load configurations based on environment
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(); // Load User Secrets in ALL environments

            #endregion

            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region Register CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(ApiConstants.CorsPolicies.AllowAllPolicy, builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });
            #endregion

            #region Register Rate Limiter
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddFixedWindowLimiter(policyName: "fixed", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 5;
                });
            });

            #endregion

            #region Register Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;

                // SignIn settings
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            #endregion

            #region Register Auth
            // Configure Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateAudience = true,
                     ValidAudience = builder.Configuration["Token:Access:Audience"],
                     ValidateIssuer = true,
                     ValidIssuer = builder.Configuration["Token:Access:Issuer"],
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,

                     IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Token:Access:SecurityKey"]!)),

                     LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,

                     NameClaimType = ClaimTypes.Name,
                     RoleClaimType = ClaimTypes.Role
                 };
             });
            #endregion

            #region Add AuthorizationBuilder
            builder.Services.AddAuthorizationBuilder()
            .AddPolicy(ApiConstants.AuthPolicies.OwnerPolicy, policy => policy.RequireAuthenticatedUser().RequireRole("SuperAdmin"));

            #endregion

            #region Register DbContext
            // Add DbContext Interceptors 
            builder.Services.AddScoped<CustomSaveChangesInterceptor>();

            if (!builder.Environment.IsEnvironment("Testing"))
            {
                builder.Services.AddDbContext<AppDbContext>((sp, options) =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlOptions => sqlOptions
                    .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
                    .EnableSensitiveDataLogging();

                    options.AddInterceptors(sp.GetRequiredService<CustomSaveChangesInterceptor>());
                });
            }

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
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IContactEmailService, ContactEmailService>();

            builder.Services.AddScoped<IJwtSession, JwtSession>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IContactService, ContactService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IAchievementService, AchievementService>();
            builder.Services.AddScoped<ISkillService, SkillService>();

            #region Storage Service
            builder.Services.AddScoped<IStorageService, StorageService>();

            builder.Services.AddScoped<IStorage, LocalStorage>();
            #endregion

            #region Register Decorators
            builder.Services.Decorate<IAuthService, AuthServiceValidationDecorator>();
            #endregion
            #endregion;

            #region Exception Handling
            // Add ProblemDetails services
            builder.Services.AddProblemDetails();

            // Add Exception Handler
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            #endregion

            #region Register Options
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("Token"));
            #endregion
        }

        public static void AddMiddlewares(this WebApplication app)
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
        }
    }
}
