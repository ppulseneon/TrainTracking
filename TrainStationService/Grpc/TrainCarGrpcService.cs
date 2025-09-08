using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TrainCarGrpcService;
using TrainStationService.Services.Interfaces;

namespace TrainStationService.Grpc;

/// <summary>
/// gRPC сервис для работы с вагонами
/// </summary>
public class TrainCarGrpcService(
    ITrainCarService trainCarService, 
    ILogger<TrainCarGrpcService> logger)
    : TrainCarService.TrainCarServiceBase
{
    public override async Task<GetTrainCarsResponse> GetTrainCars(GetTrainCarsRequest request, ServerCallContext context)
    {
        try
        {
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
}
