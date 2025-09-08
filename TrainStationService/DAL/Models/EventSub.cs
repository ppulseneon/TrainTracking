using System.ComponentModel.DataAnnotations.Schema;
using TrainStationService.DAL.Enums;

namespace TrainStationService.DAL.Models;

/// <summary>
/// Событтие объезда состава
/// </summary>
public class EventSub
{
    /// <summary>
    /// Время события
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Номер пути
    /// </summary>
    public int IdPath { get; set; }

    /// <summary>
    /// Направление
    /// </summary>
    public EventDirection Direction { get; set; }

    /// <summary>
    /// Навигационное свойство к пути
    /// </summary>
    [ForeignKey(nameof(IdPath))]
    public virtual Path Path { get; set; } = null!;
}
