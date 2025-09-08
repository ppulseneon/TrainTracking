using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainStationService.DAL.Enums;

namespace TrainStationService.DAL.Models;

/// <summary>
/// События добавления состава
/// </summary>
public class EventAdd
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
    /// Навигационное свойство
    /// </summary>
    [ForeignKey(nameof(IdPath))]
    public Path Path { get; set; } = null!;
}