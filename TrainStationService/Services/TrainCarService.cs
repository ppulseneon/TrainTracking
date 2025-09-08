using Microsoft.EntityFrameworkCore;
using TrainStationService.DAL;
using TrainStationService.DAL.Constants;
using TrainStationService.DAL.Mappers;
using TrainStationService.DAL.Models;
using TrainStationService.Dto;
using TrainStationService.Services.Interfaces;

namespace TrainStationService.Services;

/// <inheritdoc/>
public class TrainCarService(
    TrainStationDbContext context, 
    ILogger<TrainCarService> logger)
    : ITrainCarService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<TrainCarDto>> GetTrainCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Получение данных о вагонах за период с {StartDate} по {EndDate}", startDate, endDate);

            var stationData = await GetDataFromDatabase(startDate, endDate, cancellationToken);

            var epcLookup = stationData.Epcs.ToDictionary(e => e.Id, e => e.Number);

            var trainCarDtos = stationData.EpcEvents
                .Where(e => epcLookup.ContainsKey(e.IdEpc))
                .GroupBy(e => epcLookup[e.IdEpc])
                .ToTrainCarDtos(stationData.Arrivals, stationData.Departures)
                .ToList();

            logger.LogInformation("Найдено {Count} вагонов за указанный период", trainCarDtos.Count);

            return trainCarDtos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении данных о вагонах");
            throw;
        }
    }
    
    private async Task<TrainStationData> GetDataFromDatabase(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
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

        return new TrainStationData
        {
            Arrivals = arrivals,
            Departures = departures,
            EpcEvents = epcEvents,
            Epcs = epcs
        };
    }
    
    private class TrainStationData
    {
        public List<EventArrival> Arrivals { get; init; } = [];
        public List<EventDeparture> Departures { get; init; } = [];
        public List<EpcEvent> EpcEvents { get; init; } = [];
        public List<Epc> Epcs { get; init; } = [];
    }
}
