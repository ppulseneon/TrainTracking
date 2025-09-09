using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TrainStationService.DAL;
using TrainStationService.DataAccess;
using TrainStationService.DataAccess.Interfaces;
using TrainStationService.Interceptors;
using TrainStationService.Services;
using TrainStationService.Services.Interfaces;
using TrainStationService.Validators;

namespace TrainStationService.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTrainStationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddGrpcServices();
        services.AddApplicationServices();
        services.AddValidationServices();
        
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
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ValidationInterceptor>();
        });
        
        services.AddSingleton<ValidationInterceptor>();
        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITrainStationDataService, TrainStationDataService>();
        services.AddScoped<ITrainCarService, TrainCarService>();
        return services;
    }

    private static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<GetTrainCarsRequestValidator>();
        return services;
    }
}
