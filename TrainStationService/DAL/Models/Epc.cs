using System.ComponentModel.DataAnnotations;
using TrainStationService.DAL.Enums;

namespace TrainStationService.DAL.Models;

/// <summary>
/// Информация о составе
/// </summary>
public class Epc
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Инвентарный номер
    /// </summary>
    [Required]
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Тип ЕПС
    /// </summary>
    public EpcType Type { get; set; }
}