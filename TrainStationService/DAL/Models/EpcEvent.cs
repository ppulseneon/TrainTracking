using TrainStationService.DAL.Enums;

namespace TrainStationService.DAL.Models;

/// <summary>
/// События на станции
/// </summary>
public class EpcEvent
{
    /// <summary>
    /// Время события
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Номер пути, на котором произошло событие
    /// </summary>
    public int IdPath { get; set; }

    /// <summary>
    /// Вид события
    /// </summary>
    public EpcEventType Type { get; set; }

    /// <summary>
    /// Номер ЕПС в составе по порядку
    /// </summary>
    public int NumberInOrder { get; set; }

    /// <summary>
    /// Идентификатор ЕПС
    /// </summary>
    public int IdEpc { get; set; }
    
    public virtual Path Path { get; set; } = null!;
    
    public virtual Epc Epc { get; set; } = null!;
}