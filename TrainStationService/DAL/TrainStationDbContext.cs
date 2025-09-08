using Microsoft.EntityFrameworkCore;
using TrainStationService.DAL.Models;
using Path = TrainStationService.DAL.Models.Path;

namespace TrainStationService.DAL;

public class TrainStationDbContext(DbContextOptions<TrainStationDbContext> options) : DbContext(options)
{
    public DbSet<Park> Parks { get; set; }
    
    public DbSet<Path> Paths { get; set; }
    
    public DbSet<Epc> Epcs { get; set; }
    
    public DbSet<EpcEvent> EpcEvents { get; set; }
    
    public DbSet<EventArrival> EventArrivals { get; set; }
    public DbSet<EventDeparture> EventDepartures { get; set; }
    
    public DbSet<EventAdd> EventAdds { get; set; }
    public DbSet<EventSub> EventSubs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrainStationDbContext).Assembly);
    }
}