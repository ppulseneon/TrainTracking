using TrainStationService.DAL.Models;

namespace TrainStationService.DataAccess.Utilities;

public static class EventSearchUtilities
{
    public static DateTime? FindNearestArrival(DateTime eventTime, IEnumerable<EventArrival> arrivals, int timeWindowHours = 1)
    {
        return arrivals
            .Where(a => a.Time <= eventTime.AddHours(timeWindowHours) && a.Time >= eventTime.AddHours(-timeWindowHours))
            .OrderBy(a => Math.Abs((a.Time - eventTime).TotalMinutes))
            .FirstOrDefault()?.Time;
    }
    
    public static DateTime? FindNearestDeparture(DateTime eventTime, IEnumerable<EventDeparture> departures, int timeWindowHours = 1)
    {
        return departures
            .Where(d => d.Time >= eventTime.AddHours(-timeWindowHours) && d.Time <= eventTime.AddHours(timeWindowHours))
            .OrderBy(d => Math.Abs((d.Time - eventTime).TotalMinutes))
            .FirstOrDefault()?.Time;
    }
}
