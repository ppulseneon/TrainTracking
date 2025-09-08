using TrainStationService.Dto;

namespace TrainStationService.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с данными о вагонах
/// </summary>
public interface ITrainCarService
{
    /// <summary>
    /// Получить данные о вагонах за указанный период времени
    /// </summary>
    /// <param name="startDate">Начальная дата</param>
    /// <param name="endDate">Конечная дата</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список вагонов</returns>
    Task<IEnumerable<TrainCarDto>> GetTrainCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
