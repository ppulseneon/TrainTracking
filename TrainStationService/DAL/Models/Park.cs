using TrainStationService.DAL.Enums;

namespace TrainStationService.DAL.Models;

/// <summary>
/// Парки станции
/// </summary>
public class Park
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название парка
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Номер во внешней системе
    /// </summary>
    public int AsuNumber { get; set; }

    /// <summary>
    /// Тип парка
    /// </summary>
    public ParkType Type { get; set; }

    /// <summary>
    /// Направление движения
    /// </summary>
    public MovementDirection Direction { get; set; }
    
    /// <summary>
    /// Список путей
    /// </summary>
    public virtual ICollection<Path> Paths { get; set; } = new List<Path>();
}
