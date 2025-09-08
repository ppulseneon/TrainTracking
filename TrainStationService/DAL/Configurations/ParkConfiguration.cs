using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainStationService.DAL.Models;

namespace TrainStationService.DAL.Configurations;

public class ParkConfiguration : IEntityTypeConfiguration<Park>
{
    public void Configure(EntityTypeBuilder<Park> builder)
    {
        builder.ToTable("Park");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id).IsRequired();

        builder.Property(e => e.Name).IsRequired();
            
        builder.Property(e => e.AsuNumber).IsRequired();
            
        builder.Property(e => e.Type).IsRequired();
            
        builder.Property(e => e.Direction).IsRequired();
    }
}
