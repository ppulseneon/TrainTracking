using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainStationService.DAL.Models;

namespace TrainStationService.DAL.Configurations;

public class EventDepartureConfiguration : IEntityTypeConfiguration<EventDeparture>
{
    public void Configure(EntityTypeBuilder<EventDeparture> builder)
    {
        builder.ToTable("EventDeparture");
        builder.HasKey(e => new { e.Time, e.IdPath, e.TrainNumber });
        
        builder.Property(e => e.Time).IsRequired();
            
        builder.Property(e => e.IdPath).IsRequired();

        builder.Property(e => e.TrainNumber)
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(e => e.TrainIndex)
            .HasMaxLength(17).IsRequired();
    }
}
