using TrainStationService.DAL.Models;
using TrainStationService.Dto;

namespace TrainStationService.DAL.Mappers;

public static class MappingExtensions
{
    public static IEnumerable<TrainCarDto> ToTrainCarDtos(this IEnumerable<IGrouping<string, EpcEvent>> groupedEpcEvents,
        IEnumerable<EventArrival> arrivals, IEnumerable<EventDeparture> departures)
    {
        var arrivalsList = arrivals.ToList();
        var departuresList = departures.ToList();

        return groupedEpcEvents
            .Select(group => TrainCarMapper.ToDto(
                group.Key, 
                group, 
                arrivalsList, 
                departuresList))
            .Where(dto => dto.ArrivalTime.HasValue || dto.DepartureTime.HasValue);
    }
    
    public static IQueryable<EventArrival> FilterByTimeRange(this IQueryable<EventArrival> arrivals,
        DateTime startDate, DateTime endDate) => arrivals.Where(a => a.Time >= startDate && a.Time <= endDate);
    
    public static IQueryable<EventDeparture> FilterByTimeRange(this IQueryable<EventDeparture> departures,
        DateTime startDate, DateTime endDate) => departures.Where(d => d.Time >= startDate && d.Time <= endDate);
    
    public static IQueryable<EpcEvent> FilterByTimeRange(this IQueryable<EpcEvent> epcEvents,
        DateTime startDate, DateTime endDate) => epcEvents.Where(e => e.Time >= startDate && e.Time <= endDate);
}