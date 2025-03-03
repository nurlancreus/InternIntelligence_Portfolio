using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail.Templates;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Mail;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Storage;
using InternIntelligence_Portfolio.Application.Abstractions.Services.Token;
using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.Decorators;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Services;
using InternIntelligence_Portfolio.Infrastructure.Services.Mail.Templates;
using InternIntelligence_Portfolio.Infrastructure.Services.Mail;
using InternIntelligence_Portfolio.Infrastructure.Services.Sessions;
using InternIntelligence_Portfolio.Infrastructure.Services.Storage.Local;
using InternIntelligence_Portfolio.Infrastructure.Services.Storage;
using InternIntelligence_Portfolio.Infrastructure.Services.Token;

namespace InternIntelligence_Portfolio.API.Configurations
{
    public static class ServiceConfigurations
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
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
            builder.Services.Decorate<IUserService, UserServiceValidationDecorator>();
            builder.Services.Decorate<IProjectService, ProjectServiceValidationDecorator>();
            builder.Services.Decorate<ISkillService, SkillServiceValidationDecorator>();
            builder.Services.Decorate<IAchievementService, AchievementServiceValidationDecorator>();
            builder.Services.Decorate<IContactService, ContactServiceValidationDecorator>();
            #endregion

            return builder;
        }
    }
}
