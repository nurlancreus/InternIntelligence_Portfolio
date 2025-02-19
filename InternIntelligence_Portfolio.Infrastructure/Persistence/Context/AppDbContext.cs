using InternIntelligence_Portfolio.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using InternIntelligence_Portfolio.Domain.Entities;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Configurations;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Context
{
    // add-migration init -OutputDir Persistence/Context/Migrations
    public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(dbContextOptions)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationUserConfiguration))!);

            base.OnModelCreating(builder);
        }

        #region Files (TPH)
        public DbSet<ApplicationFile> ApplicationFiles { get; set; }
        public DbSet<UserProfilePictureFile> UserProfilePictureFiles { get; set; }
        public DbSet<ProjectCoverImageFile> ProjectCoverImageFiles { get; set; }
        #endregion

        public DbSet<Project> Projects { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
