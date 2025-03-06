using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace InternIntelligence_Portfolio.API.Configurations
{
    public static class AuthConfigurations
    {
        public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
        {
            var a = builder.Configuration["Token:Access:Audience"];
            var b = builder.Configuration["Token:Access:Issuer"];
            var c = builder.Configuration["Token:Access:SecurityKey"];
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

            #region Add AuthorizationBuilder
            builder.Services.AddAuthorizationBuilder()
            .AddPolicy(ApiConstants.AuthPolicies.OwnerPolicy, policy => policy.RequireAuthenticatedUser().RequireRole("SuperAdmin"));

            #endregion;

            return builder;
        }
    }
}
