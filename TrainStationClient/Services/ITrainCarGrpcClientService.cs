using TrainCarGrpcService;

namespace TrainStationClient.Services;

/// <summary>
/// Интерфейс для сервиса работы с gRPC клиентом для получения данных о вагонах
/// </summary>
public interface ITrainCarGrpcClientService
{
    /// <summary>
    /// Получить данные о вагонах за указанный период
    /// </summary>
    /// <param name="startDate">Дата начала периода</param>
    /// <param name="endDate">Дата окончания периода</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список вагонов</returns>
    Task<GetTrainCarsResponse> GetTrainCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
