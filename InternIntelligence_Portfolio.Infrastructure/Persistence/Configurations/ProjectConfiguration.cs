using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Entities;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
               .HasMaxLength(DomainConstants.Project.NameMaxLength);

            builder.Property(p => p.Description)
                .HasMaxLength(DomainConstants.Project.DescriptionMaxLength);

            builder
                .HasOne(p => p.CoverImageFile)
                .WithOne(ci => ci.Project)
                .HasForeignKey<ProjectCoverImageFile>(ci => ci.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}
