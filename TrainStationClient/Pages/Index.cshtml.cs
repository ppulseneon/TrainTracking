using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainCarGrpcService;
using TrainStationClient.Services;

namespace TrainStationClient.Pages;

public class IndexModel(ILogger<IndexModel> logger, ITrainCarGrpcClientService trainCarService)
    : PageModel
{
    [BindProperty]
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);

    [BindProperty]
    public DateTime EndDate { get; set; } = DateTime.Today;

    public List<TrainCar> TrainCars { get; set; } = [];

    public List<string> Errors { get; set; } = [];

    public bool ShowResults { get; set; }

    [BindProperty]
    public string? SelectedCarNumber { get; set; }

    public TrainCarPathInfo? CarPathInfo { get; set; }

    public bool ShowPathDetails { get; set; }

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
                Errors = ["Дата начала не может быть больше даты окончания"];
                return Page();
            }

            logger.LogInformation("Запрос данных о вагонах с {StartDate} по {EndDate}", StartDate, EndDate);

            var response = await trainCarService.GetTrainCarsAsync(StartDate, EndDate);
            TrainCars = response.TrainCars.ToList();
            ShowResults = true;

            logger.LogInformation("Получено {Count} вагонов", TrainCars.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении данных о вагонах");
            Errors = ParseValidationErrors(ex.Message);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostGetPathsAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(SelectedCarNumber))
            {
                Errors = ["Обязательно нужно указать номер вагона"];
                return Page();
            }

            logger.LogInformation("Запрос путей для вагона {CarNumber} с {StartDate} по {EndDate}", SelectedCarNumber, StartDate, EndDate);

            var response = await trainCarService.GetTrainCarPathsAsync(SelectedCarNumber, StartDate, EndDate);
            CarPathInfo = response.PathInfo;
            ShowPathDetails = true;

            if (CarPathInfo != null)
            {
                logger.LogInformation("Получена информация о {PathCount} путях для вагона {CarNumber}", 
                    CarPathInfo.PathStays.Count, SelectedCarNumber);
            }
            else
            {
                Errors = [$"Информация о путях для вагона {SelectedCarNumber} не была найдена"];
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении информации о путях для вагона №{CarNumber}", SelectedCarNumber);
            Errors = ParseValidationErrors(ex.Message);
        }

        return Page();
    }
    
    private static List<string> ParseValidationErrors(string errorMessage)
    {
        var errors = new List<string>();
        
        if (errorMessage.Contains("Validation error:"))
        {
            var validationPart = errorMessage[(errorMessage.IndexOf("Validation error:", StringComparison.Ordinal) + "Validation error:".Length)..].Trim();
            
            errors = validationPart.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .ToList();
        }
        else
        {
            errors.Add(errorMessage);
        }

        return errors;
    }
}