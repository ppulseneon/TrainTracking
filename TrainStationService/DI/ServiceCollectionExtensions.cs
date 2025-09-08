using Microsoft.EntityFrameworkCore;
using TrainStationService.DAL;
using TrainStationService.Services;
using TrainStationService.Services.Interfaces;

namespace TrainStationService.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTrainStationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddGrpcServices();
        services.AddApplicationServices();
        
        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TrainStationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging();
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });
        
        return services;
    }

    private static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();
        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITrainCarService, TrainCarService>();
        return services;
    }
}
