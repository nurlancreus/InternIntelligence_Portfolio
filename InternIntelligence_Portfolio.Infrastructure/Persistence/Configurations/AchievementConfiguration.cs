using InternIntelligence_Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InternIntelligence_Portfolio.Domain;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Configurations
{
    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .HasMaxLength(DomainConstants.Achievement.TitleMaxLength);

            builder.Property(a => a.Description)
                .HasMaxLength(DomainConstants.Achievement.DescriptionMaxLength);
        }
    }
}
