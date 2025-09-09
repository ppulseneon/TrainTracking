using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TrainCarGrpcService;
using TrainStationService.Exceptions;
using TrainStationService.Services.Interfaces;

namespace TrainStationService.Grpc;

/// <summary>
/// gRPC сервис для работы с вагонами
/// </summary>
public class TrainCarGrpcService(
    ITrainCarService trainCarService,
    IValidator<GetTrainCarsRequest> trainCarsRequestValidator,
    IValidator<GetTrainCarPathsRequest> trainCarPathsRequestValidator,
    ILogger<TrainCarGrpcService> logger)
    : TrainCarService.TrainCarServiceBase
{
    public override async Task<GetTrainCarsResponse> GetTrainCars(GetTrainCarsRequest request, ServerCallContext context)
    {
        try
        {
            // Валидация запроса
            var validationResult = await trainCarsRequestValidator.ValidateAsync(request, context.CancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                
                throw new TrainStationService.Exceptions.ValidationException(errors);
            }

            var startDate = request.StartDate.ToDateTime();
            var endDate = request.EndDate.ToDateTime();

            logger.LogInformation("Обработка запроса на получение рейсов вагонов в период с {StartDate} по {EndDate}", startDate, endDate);

            var trainCars = await trainCarService.GetTrainCarsAsync(startDate, endDate, context.CancellationToken);

            var response = new GetTrainCarsResponse();

            foreach (var car in trainCars)
            {
                var trainCar = new TrainCar
                {
                    CarNumber = car.CarNumber,
                    ArrivalTime = car.ArrivalTime.HasValue 
                        ? Timestamp.FromDateTime(car.ArrivalTime.Value.ToUniversalTime()) 
                        : null,
                    DepartureTime = car.DepartureTime.HasValue 
                        ? Timestamp.FromDateTime(car.DepartureTime.Value.ToUniversalTime()) 
                        : null
                };

                response.TrainCars.Add(trainCar);
            }

            logger.LogInformation("Найдено {Count} вагонов в выборке", response.TrainCars.Count);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обработке запроса GetTrainCars");
            throw;
        }
    }

    public override async Task<GetTrainCarPathsResponse> GetTrainCarPaths(GetTrainCarPathsRequest request, ServerCallContext context)
    {
        try
        {
            // Валидация запроса
            var validationResult = await trainCarPathsRequestValidator.ValidateAsync(request, context.CancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                
                throw new TrainStationService.Exceptions.ValidationException(errors);
            }

            var startDate = request.StartDate.ToDateTime();
            var endDate = request.EndDate.ToDateTime();

            logger.LogInformation("Обработка запроса на получение путей для вагона {CarNumber} в период с {StartDate} по {EndDate}", 
                request.CarNumber, startDate, endDate);

            var pathInfo = await trainCarService.GetTrainCarPathsAsync(request.CarNumber, startDate, endDate, context.CancellationToken);

            var response = new GetTrainCarPathsResponse();

            if (pathInfo != null)
            {
                var trainCarPathInfo = new TrainCarPathInfo
                {
                    CarNumber = pathInfo.CarNumber,
                    ArrivalTime = pathInfo.ArrivalTime.HasValue 
                        ? Timestamp.FromDateTime(pathInfo.ArrivalTime.Value.ToUniversalTime()) 
                        : null,
                    DepartureTime = pathInfo.DepartureTime.HasValue 
                        ? Timestamp.FromDateTime(pathInfo.DepartureTime.Value.ToUniversalTime()) 
                        : null
                };

                foreach (var pathStay in pathInfo.PathStays)
                {
                    var pathStayInfo = new PathStayInfo
                    {
                        PathNumber = pathStay.PathNumber,
                        StartTime = Timestamp.FromDateTime(pathStay.StartTime.ToUniversalTime()),
                        EndTime = Timestamp.FromDateTime(pathStay.EndTime.ToUniversalTime()),
                        ParkName = pathStay.ParkName
                    };

                    trainCarPathInfo.PathStays.Add(pathStayInfo);
                }

                response.PathInfo = trainCarPathInfo;
            }

            logger.LogInformation("Найдено {PathCount} путей для вагона {CarNumber}", 
                pathInfo?.PathStays.Count ?? 0, request.CarNumber);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обработке запроса GetTrainCarPaths для вагона {CarNumber}", request.CarNumber);
            throw;
        }
    }
}
