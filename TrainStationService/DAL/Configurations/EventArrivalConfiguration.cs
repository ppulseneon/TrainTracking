using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainStationService.DAL.Models;

namespace TrainStationService.DAL.Configurations;

public class EventArrivalConfiguration : IEntityTypeConfiguration<EventArrival>
{
    public void Configure(EntityTypeBuilder<EventArrival> builder)
    {
        builder.ToTable("EventArrival");
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
