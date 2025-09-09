using TrainStationService.DAL.Models;

namespace TrainStationService.DataAccess.Models;

/// <summary>
/// Модель данных станции за определенный период
/// </summary>
public class TrainStationData
{
    /// <summary>
    /// События прибытия поездов
    /// </summary>
    public List<EventArrival> Arrivals { get; init; } = [];

    /// <summary>
    /// События отправления поездов
    /// </summary>
    public List<EventDeparture> Departures { get; init; } = [];

    /// <summary>
    /// События ЕПС на станции
    /// </summary>
    public List<EpcEvent> EpcEvents { get; init; } = [];

    /// <summary>
    /// Информация о вагонах
    /// </summary>
    public List<Epc> Epcs { get; init; } = [];
}
