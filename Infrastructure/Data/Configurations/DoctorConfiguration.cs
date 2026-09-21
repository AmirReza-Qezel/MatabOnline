using Domain.DoctorAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            builder.Property(d => d.FullName).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Specialty).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Info).HasMaxLength(2000);
            builder.Property(d => d.ClinicPhoneNumber).HasMaxLength(20).IsRequired();

            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // don't cascade-delete appointment history
        }
    }
}
