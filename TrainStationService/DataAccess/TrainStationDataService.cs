using Microsoft.EntityFrameworkCore;
using TrainStationService.DAL;
using TrainStationService.DAL.Constants;
using TrainStationService.DAL.Mappers;
using TrainStationService.DataAccess.Interfaces;
using TrainStationService.DataAccess.Models;

namespace TrainStationService.DataAccess;

public class TrainStationDataService(
    TrainStationDbContext context,
    ILogger<TrainStationDataService> logger)
    : ITrainStationDataService
{
    /// <inheritdoc/>
    public async Task<TrainStationData> GetStationDataAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Выгрущзка данных за период с {StartDate} по {EndDate}", startDate, endDate);

            var arrivals = await context.EventArrivals
                .AsNoTracking()
                .FilterByTimeRange(startDate, endDate)
                .ToListAsync(cancellationToken);

            var departures = await context.EventDepartures
                .AsNoTracking()
                .FilterByTimeRange(startDate, endDate)
                .ToListAsync(cancellationToken);

            var epcEvents = await context.EpcEvents
                .AsNoTracking()
                .FilterByTimeRange(startDate, endDate)
                .ToListAsync(cancellationToken);

            var epcs = await context.Epcs
                .AsNoTracking()
                .Where(e => e.Type == DAL.Enums.EpcType.Car && e.Number != NumbersConstants.NullNumber)
                .ToListAsync(cancellationToken);

            var result = new TrainStationData
            {
                Arrivals = arrivals,
                Departures = departures,
                EpcEvents = epcEvents,
                Epcs = epcs
            };

            logger.LogInformation("Выгружено из бд {ArrivalsCount} прибытий, {DeparturesCount} отправлений, {EventsCount} событий ЕПС, {EpcsCount} вагонов",
                arrivals.Count, departures.Count, epcEvents.Count, epcs.Count);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении данных станции с {StartDate} по {EndDate}", startDate, endDate);
            throw;
        }
    }
}
