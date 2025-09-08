using TrainStationService.DAL.Models;
using TrainStationService.Dto;

namespace TrainStationService.DAL.Mappers;

public static class TrainCarMapper
{
    public static TrainCarDto ToDto(
        string carNumber,
        IEnumerable<EpcEvent> epcEvents,
        IEnumerable<EventArrival> arrivals,
        IEnumerable<EventDeparture> departures)
    {
        var events = epcEvents.ToList();
        
        if (events.Count == 0)
        {
            return new TrainCarDto { CarNumber = carNumber };
        }

        var firstEventTime = events.Min(e => e.Time);
        var lastEventTime = events.Max(e => e.Time);

        var arrivalTime = FindNearestArrival(firstEventTime, arrivals);
        var departureTime = FindNearestDeparture(lastEventTime, departures);

        return new TrainCarDto
        {
            CarNumber = carNumber,
            ArrivalTime = arrivalTime,
            DepartureTime = departureTime
        };
    }
    
    private static DateTime? FindNearestArrival(DateTime eventTime, IEnumerable<EventArrival> arrivals)
    {
        return arrivals
            .Where(a => a.Time <= eventTime.AddHours(1) && a.Time >= eventTime.AddHours(-1))
            .OrderBy(a => Math.Abs((a.Time - eventTime).TotalMinutes))
            .FirstOrDefault()?.Time;
    }
    
    private static DateTime? FindNearestDeparture(DateTime eventTime, IEnumerable<EventDeparture> departures)
    {
        return departures
            .Where(d => d.Time >= eventTime.AddHours(-1) && d.Time <= eventTime.AddHours(1))
            .OrderBy(d => Math.Abs((d.Time - eventTime).TotalMinutes))
            .FirstOrDefault()?.Time;
    }
}
