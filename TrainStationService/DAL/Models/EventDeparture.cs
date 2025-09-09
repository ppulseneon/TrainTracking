using System.ComponentModel.DataAnnotations.Schema;

namespace TrainStationService.DAL.Models;

/// <summary>
/// Информация об отправлениях со станции
/// </summary>
public class EventDeparture
{
    /// <summary>
    /// Время отправления
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
    
    /// <summary>
    /// Навигационное свойство
    /// </summary>
    [ForeignKey(nameof(IdPath))]
    public virtual Path Path { get; set; } = null!;
}