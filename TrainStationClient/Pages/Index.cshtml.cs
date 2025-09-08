using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainCarGrpcService;
using TrainStationClient.Services;

namespace TrainStationClient.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ITrainCarGrpcClientService _trainCarService;

    public IndexModel(ILogger<IndexModel> logger, ITrainCarGrpcClientService trainCarService)
    {
        _logger = logger;
        _trainCarService = trainCarService;
    }

    [BindProperty]
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);

    [BindProperty]
    public DateTime EndDate { get; set; } = DateTime.Today;

    public List<TrainCar> TrainCars { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public bool ShowResults { get; set; }

    public void OnGet()
    {
        // Инициализация значений по умолчанию
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (StartDate > EndDate)
            {
                ErrorMessage = "Дата начала не может быть больше даты окончания";
                return Page();
            }

            _logger.LogInformation("Запрос данных о вагонах с {StartDate} по {EndDate}", StartDate, EndDate);

            var response = await _trainCarService.GetTrainCarsAsync(StartDate, EndDate);
            TrainCars = response.TrainCars.ToList();
            ShowResults = true;

            _logger.LogInformation("Получено {Count} вагонов", TrainCars.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении данных о вагонах");
            ErrorMessage = $"Ошибка при получении данных: {ex.Message}";
        }

        return Page();
    }
}