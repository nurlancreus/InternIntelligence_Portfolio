using InternIntelligence_Portfolio.Domain;
using InternIntelligence_Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .HasMaxLength(DomainConstants.Contact.FirstNameMaxLength);

            builder.Property(c => c.LastName)
                .HasMaxLength(DomainConstants.Contact.LastNameMaxLength);

            builder.Property(c => c.Email)
                .HasMaxLength(DomainConstants.Contact.EmailMaxLength);

            builder.Property(c => c.Message)
                .HasMaxLength(DomainConstants.Contact.MessageMaxLength);

            builder.Property(c => c.Subject)
                .HasConversion<string>();
        }
    }
}
