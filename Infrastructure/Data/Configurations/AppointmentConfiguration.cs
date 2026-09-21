using Domain.AppointmentAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");

            builder.Property(a => a.PatientName).HasMaxLength(100).IsRequired();
            builder.Property(a => a.PatientPhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(a => a.AppointmentDate).IsRequired();

            builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate });
        }
    }
}
