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

    /// <summary>
    /// Получить информацию о путях, на которых находился конкретный вагон за период
    /// </summary>
    /// <param name="carNumber">Номер вагона</param>
    /// <param name="startDate">Начальная дата</param>
    /// <param name="endDate">Конечная дата</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о путях вагона</returns>
    Task<TrainCarPathInfoDto?> GetTrainCarPathsAsync(string carNumber, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
