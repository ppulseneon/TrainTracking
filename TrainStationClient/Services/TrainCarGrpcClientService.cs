using Google.Protobuf.WellKnownTypes;
using TrainCarGrpcService;

namespace TrainStationClient.Services;

public class TrainCarGrpcClientService(
    TrainCarService.TrainCarServiceClient grpcClient,
    ILogger<TrainCarGrpcClientService> logger)
    : ITrainCarGrpcClientService
{
    /// <summary>
    /// Получить данные о вагонах за указанный период
    /// </summary>
    public async Task<GetTrainCarsResponse> GetTrainCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Отправка gRPC запроса для получения данных о вагонах с {StartDate} по {EndDate}", startDate, endDate);

            var request = new GetTrainCarsRequest
            {
                StartDate = Timestamp.FromDateTime(startDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(endDate.ToUniversalTime())
            };

            var response = await grpcClient.GetTrainCarsAsync(request, cancellationToken: cancellationToken);
            
            logger.LogInformation("Получен ответ с {Count} вагонами", response.TrainCars.Count);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при выполнении gRPC запроса GetTrainCars");
            throw;
        }
    }

    /// <summary>
    /// Получить информацию о путях конкретного вагона за указанный период
    /// </summary>
    public async Task<GetTrainCarPathsResponse> GetTrainCarPathsAsync(string carNumber, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Отправка gRPC запроса для получения путей вагона {CarNumber} с {StartDate} по {EndDate}", carNumber, startDate, endDate);

            var request = new GetTrainCarPathsRequest
            {
                CarNumber = carNumber,
                StartDate = Timestamp.FromDateTime(startDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(endDate.ToUniversalTime())
            };

            var response = await grpcClient.GetTrainCarPathsAsync(request, cancellationToken: cancellationToken);
            
            logger.LogInformation("Получен ответ с информацией {PathCount}  о путях для вагона {CarNumber}", 
                response.PathInfo?.PathStays.Count ?? 0, carNumber);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при выполнении gRPC запроса GetTrainCarPaths для вагона {CarNumber}", carNumber);
            throw;
        }
    }
}
