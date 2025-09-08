using System.ComponentModel.DataAnnotations.Schema;

namespace TrainStationService.DAL.Models;

/// <summary>
/// Информацию о путях станции
/// </summary>
public class Path
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер во внешней системе
    /// </summary>

    public string AsuNumber { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор парка
    /// </summary>
    public int IdPark { get; set; }
    
    [ForeignKey(nameof(IdPark))]
    public virtual Park Park { get; set; } = null!;

    public virtual ICollection<EpcEvent> EpcEvents { get; set; } = new List<EpcEvent>();
    public virtual ICollection<EventArrival> EventArrivals { get; set; } = new List<EventArrival>();
    public virtual ICollection<EventDeparture> EventDepartures { get; set; } = new List<EventDeparture>();
    public virtual ICollection<EventAdd> EventAdds { get; set; } = new List<EventAdd>();
    public virtual ICollection<EventSub> EventSubs { get; set; } = new List<EventSub>();
}
