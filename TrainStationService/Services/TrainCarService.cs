using Microsoft.EntityFrameworkCore;
using TrainStationService.DAL;
using TrainStationService.DAL.Mappers;
using TrainStationService.DataAccess.Interfaces;
using TrainStationService.DataAccess.Utilities;
using TrainStationService.Dto;
using TrainStationService.Services.Interfaces;

namespace TrainStationService.Services;

/// <inheritdoc/>
public class TrainCarService(
    TrainStationDbContext context, 
    ITrainStationDataService dataService,
    ILogger<TrainCarService> logger)
    : ITrainCarService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<TrainCarDto>> GetTrainCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Получение данных о вагонах за период с {StartDate} по {EndDate}", startDate, endDate);

            var stationData = await dataService.GetStationDataAsync(startDate, endDate, cancellationToken);

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

    /// <inheritdoc/>
    public async Task<TrainCarPathInfoDto?> GetTrainCarPathsAsync(string carNumber, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Получение информации о путях для вагона {CarNumber} за период с {StartDate} по {EndDate}", carNumber, startDate, endDate);

            var epc = await context.Epcs
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Number == carNumber && e.Type == DAL.Enums.EpcType.Car, cancellationToken);

            if (epc == null)
            {
                logger.LogWarning("Вагон с номером {CarNumber} не найден", carNumber);
                return null;
            }

            var epcEvents = await context.EpcEvents
                .AsNoTracking()
                .Include(e => e.Path)
                .ThenInclude(p => p.Park)
                .Where(e => e.IdEpc == epc.Id)
                .FilterByTimeRange(startDate, endDate)
                .OrderBy(e => e.Time)
                .ToListAsync(cancellationToken);

            if (epcEvents.Count == 0)
            {
                logger.LogInformation("События для вагона {CarNumber} в указанный период не найдены", carNumber);
                return null;
            }

            var stationData = await dataService.GetStationDataAsync(startDate, endDate, cancellationToken);
            var firstEventTime = epcEvents.Min(e => e.Time);
            var lastEventTime = epcEvents.Max(e => e.Time);

            var arrivalTime = EventSearchUtilities.FindNearestArrival(firstEventTime, stationData.Arrivals);
            var departureTime = EventSearchUtilities.FindNearestDeparture(lastEventTime, stationData.Departures);

            var pathStays = epcEvents
                .GroupBy(e => new { e.IdPath, PathNumber = e.Path.AsuNumber, ParkName = e.Path.Park.Name })
                .Select(g => new PathStayInfo
                {
                    PathNumber = g.Key.PathNumber,
                    StartTime = g.Min(e => e.Time),
                    EndTime = g.Max(e => e.Time),
                    ParkName = g.Key.ParkName
                })
                .Where(p => p.StartTime != p.EndTime)
                .OrderBy(p => p.StartTime)
                .ToList();

            var result = new TrainCarPathInfoDto
            {
                CarNumber = carNumber,
                ArrivalTime = arrivalTime,
                DepartureTime = departureTime,
                PathStays = pathStays
            };

            logger.LogInformation("Найдено {PathCount} путей для вагона {CarNumber}", pathStays.Count, carNumber);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении информации о путях для вагона {CarNumber}", carNumber);
            throw;
        }
    }
}
