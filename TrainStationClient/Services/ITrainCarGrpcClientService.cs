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

    /// <summary>
    /// Получить информацию о путях конкретного вагона за промежуток
    /// </summary>
    /// <param name="carNumber">Номер вагона</param>
    /// <param name="startDate">Дата начала</param>
    /// <param name="endDate">Дата окончания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о путях</returns>
    Task<GetTrainCarPathsResponse> GetTrainCarPathsAsync(string carNumber, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
