using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainStationService.DAL.Models;

namespace TrainStationService.DAL.Configurations;

public class EpcConfiguration : IEntityTypeConfiguration<Epc>
{
    public void Configure(EntityTypeBuilder<Epc> builder)
    {
        builder.ToTable("Epc");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id).IsRequired();

        builder.Property(e => e.Number)
            .HasMaxLength(8)
            .IsRequired();
            
        builder.Property(e => e.Type).IsRequired();
    }
}
