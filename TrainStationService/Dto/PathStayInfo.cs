namespace TrainStationService.Dto;

public class PathStayInfo
{
    /// <summary>
    /// Номер пути
    /// </summary>
    public string PathNumber { get; init; } = string.Empty;

    /// <summary>
    /// Время первого события
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Время последнего события
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Продолжительность
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// Название парка
    /// </summary>
    public string ParkName { get; init; } = string.Empty;
}
