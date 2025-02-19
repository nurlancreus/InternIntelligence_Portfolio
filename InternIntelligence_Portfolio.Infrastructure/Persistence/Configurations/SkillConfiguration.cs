using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(DomainConstants.Skill.NameMaxLength);

            builder.Property(a => a.Description)
                .HasMaxLength(DomainConstants.Skill.NameMaxLength);

            builder.Property(s => s.ProficiencyLevel)
                .HasConversion<string>();
        }
    }
}
