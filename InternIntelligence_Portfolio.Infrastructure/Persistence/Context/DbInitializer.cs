using InternIntelligence_Portfolio.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Context
{
    public static class DbInitializer
    {
        public static async Task SeedSuperAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            const string superAdminEmail = "admin@portfolio.com";
            const string superAdminPassword = "Ghujtyrtyu456$";
            const string superAdminRole = "SuperAdmin";

            if (!await roleManager.RoleExistsAsync(superAdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(superAdminRole));
            }

            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdmin == null)
            {
                superAdmin = ApplicationUser.Create("Super", "Admin", "superAdmin", superAdminEmail);

                superAdmin.EmailConfirmed = true;

                var result = await userManager.CreateAsync(superAdmin, superAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, superAdminRole);
                }
            }
        }
    }
}
