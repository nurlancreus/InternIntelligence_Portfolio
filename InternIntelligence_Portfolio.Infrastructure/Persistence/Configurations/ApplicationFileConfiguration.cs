using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Configurations
{
    public class ApplicationFileConfiguration : IEntityTypeConfiguration<ApplicationFile>
    {
        public void Configure(EntityTypeBuilder<ApplicationFile> builder)
        {
            builder.HasKey(f => f.Id);

            builder
                .HasDiscriminator(f => f.Type)
                .HasValue<UserProfilePictureFile>("ProfilePicture")
                .HasValue<ProjectCoverImageFile>("CoverImage");

            builder.Property(f => f.Name)
               .HasMaxLength(DomainConstants.File.NameMaxLength);

            builder.Property(f => f.Path)
                .HasMaxLength(DomainConstants.File.PathMaxLength);

            builder.Property(f => f.Storage)
                .HasConversion<string>();
        }
    }
}
