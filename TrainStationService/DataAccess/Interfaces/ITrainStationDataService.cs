using TrainStationService.DataAccess.Models;

namespace TrainStationService.DataAccess.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с данными станции
/// </summary>
public interface ITrainStationDataService
{
    /// <summary>
    /// Получить агрегированные данные станции за период
    /// </summary>
    /// <param name="startDate">Начальная дата</param>
    /// <param name="endDate">Конечная дата</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Данные станции за период</returns>
    Task<TrainStationData> GetStationDataAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
