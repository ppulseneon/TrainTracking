using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainStationService.DAL.Models;

namespace TrainStationService.DAL.Configurations;

public class EventAddConfiguration : IEntityTypeConfiguration<EventAdd>
{
    public void Configure(EntityTypeBuilder<EventAdd> builder)
    {
        builder.ToTable("EventAdd");
        builder.HasKey(e => new { e.Time, e.IdPath, e.Direction });
        
        builder.Property(e => e.Time)
            .HasColumnName("time");
            
        builder.Property(e => e.IdPath)
            .HasColumnName("id_path");
            
        builder.Property(e => e.Direction)
            .HasColumnName("direction");
    }
}
