using Domain.ContactMessageAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.ToTable("ContactMessages");

            builder.Property(c => c.FullName).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Message).HasMaxLength(2000).IsRequired();
        }
    }
}
