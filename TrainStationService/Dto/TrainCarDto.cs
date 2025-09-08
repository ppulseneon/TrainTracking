namespace TrainStationService.Dto;

/// <summary>
/// DTO данных о вагоне
/// </summary>
public class TrainCarDto
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
}
