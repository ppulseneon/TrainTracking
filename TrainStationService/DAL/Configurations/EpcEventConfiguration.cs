using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainStationService.DAL.Models;

namespace TrainStationService.DAL.Configurations;

public class EpcEventConfiguration : IEntityTypeConfiguration<EpcEvent>
{
    public void Configure(EntityTypeBuilder<EpcEvent> builder)
    {
        builder.ToTable("EpcEvent");
        builder.HasKey(e => new { e.Time, e.IdPath, e.IdEpc, e.NumberInOrder });
        
        builder.Property(e => e.Time).IsRequired();
            
        builder.Property(e => e.IdPath).IsRequired();
            
        builder.Property(e => e.Type).IsRequired();
            
        builder.Property(e => e.NumberInOrder).IsRequired();
            
        builder.Property(e => e.IdEpc).IsRequired();
        
        builder.HasOne(e => e.Path)
            .WithMany(p => p.EpcEvents)
            .HasForeignKey(e => e.IdPath);

        builder.HasOne(e => e.Epc)
            .WithMany()
            .HasForeignKey(e => e.IdEpc);
    }
}