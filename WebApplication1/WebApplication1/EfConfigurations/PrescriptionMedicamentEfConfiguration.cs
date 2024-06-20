using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.EfConfigurations;

public class PrescriptionMedicamentEfConfiguration : IEntityTypeConfiguration<Prescription_Medicament>
{
    public void Configure(EntityTypeBuilder<Prescription_Medicament> builder)
    {
        builder.HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
        builder.Property(pm => pm.Details).IsRequired().HasMaxLength(100);
    }
}