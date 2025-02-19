using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .HasMaxLength(DomainConstants.User.FirstNameMaxLength);

            builder.Property(u => u.LastName)
                .HasMaxLength(DomainConstants.User.LastNameMaxLength);

            builder.Property(u => u.UserName)
                .HasMaxLength(DomainConstants.User.UserNameMaxLength);

            builder.Property(u => u.Email)
                .HasMaxLength(DomainConstants.User.EmailMaxLength);

            builder.Property(u => u.Bio)
                .HasMaxLength(DomainConstants.User.BioMaxLength);

            builder
                .HasOne(u => u.ProfilePictureFile)
                .WithOne(pp => pp.User)
                .HasForeignKey<UserProfilePictureFile>(pp => pp.UserId)
                .IsRequired(false);

            builder
                .HasMany(u => u.Projects)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .IsRequired();

            builder
                .HasMany(u => u.Skills)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .IsRequired();

            builder
                .HasMany(u => u.Achievements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .IsRequired();
        }
    }
}
