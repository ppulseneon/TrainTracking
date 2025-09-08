using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainStationService.DAL.Models;

namespace TrainStationService.DAL.Configurations;

public class EventSubConfiguration : IEntityTypeConfiguration<EventSub>
{
    public void Configure(EntityTypeBuilder<EventSub> builder)
    {
        builder.ToTable("EventSub");
        builder.HasKey(e => new { e.Time, e.IdPath, e.Direction });
        
        builder.Property(e => e.Time).IsRequired();
            
        builder.Property(e => e.IdPath).IsRequired();
            
        builder.Property(e => e.Direction).IsRequired();
    }
}
