using TrainStationService.DAL.Models;
using TrainStationService.DataAccess.Utilities;
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

        var arrivalTime = EventSearchUtilities.FindNearestArrival(firstEventTime, arrivals);
        var departureTime = EventSearchUtilities.FindNearestDeparture(lastEventTime, departures);

        return new TrainCarDto
        {
            CarNumber = carNumber,
            ArrivalTime = arrivalTime,
            DepartureTime = departureTime
        };
    }
}
