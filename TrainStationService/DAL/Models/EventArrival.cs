using System.ComponentModel.DataAnnotations.Schema;

namespace TrainStationService.DAL.Models;

/// <summary>
/// Событие прибытия состава
/// </summary>
public class EventArrival
{
    /// <summary>
    /// Время прибытия
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Идентификатор пути
    /// </summary>
    public int IdPath { get; set; }

    /// <summary>
    /// Номер поезда
    /// </summary>
    public string TrainNumber { get; set; } = string.Empty;

    /// <summary>
    /// Индекс поезда
    /// </summary>
    public string? TrainIndex { get; set; }
    
    [ForeignKey(nameof(IdPath))]
    public Path Path { get; set; } = null!;
}