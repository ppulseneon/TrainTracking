using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Path = TrainStationService.DAL.Models.Path;

namespace TrainStationService.DAL.Configurations;

public class PathConfiguration : IEntityTypeConfiguration<Path>
{
    public void Configure(EntityTypeBuilder<Path> builder)
    {
        builder.ToTable("Path");
        builder.HasKey(e => e.Id);
            
        builder.Property(e => e.AsuNumber)
            .HasMaxLength(2)
            .IsRequired();
            
        builder.Property(e => e.IdPark).IsRequired();

        builder.HasOne(d => d.Park)
            .WithMany(p => p.Paths)
            .HasForeignKey(d => d.IdPark);
    }
}
