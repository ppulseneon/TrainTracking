namespace TrainStationService.Dto;

public class TrainCarPathInfoDto
{
    /// <summary>
    /// Номер вагона
    /// </summary>
    public string CarNumber { get; init; } = string.Empty;

    /// <summary>
    /// Время прибытия
    /// </summary>
    public DateTime? ArrivalTime { get; init; }

    /// <summary>
    /// Время отправления
    /// </summary>
    public DateTime? DepartureTime { get; init; }

    /// <summary>
    /// Список путей со временем пребывания
    /// </summary>
    public List<PathStayInfo> PathStays { get; init; } = [];
}