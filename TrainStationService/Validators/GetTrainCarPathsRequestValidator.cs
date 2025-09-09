using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using TrainCarGrpcService;

namespace TrainStationService.Validators;

public class GetTrainCarPathsRequestValidator : AbstractValidator<GetTrainCarPathsRequest>
{
    public GetTrainCarPathsRequestValidator()
    {
        RuleFor(x => x.CarNumber)
            .NotEmpty()
            .WithMessage("Номер вагона обязателен")
            .Length(1, 20)
            .WithMessage("Номер вагона должен содержать от 1 до 20 символов")
            .Matches("^[0-9]+$")
            .WithMessage("Номер вагона может содержать только цифры");

        RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage("Обязательна дата начала")
            .Must(BeValidTimestamp)
            .WithMessage("Дата начала с неверным форматом");

        RuleFor(x => x.EndDate)
            .NotNull()
            .WithMessage("Обязательна дата окончания")
            .Must(BeValidTimestamp)
            .WithMessage("Дата окончания с неверным форматом");

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("Дата начала не может быть позже даты окончания и не может быть больше текущей даты");
    }
    
    private static bool BeValidTimestamp(Timestamp? timestamp)
    {
        if (timestamp == null) return false;
        
        var dateTime = timestamp.ToDateTime();
        return dateTime >= new DateTime(1900, 1, 1) && dateTime <= DateTime.UtcNow.AddDays(1);
    }
    
    private static bool HaveValidDateRange(GetTrainCarPathsRequest request)
    {
        var now = DateTime.UtcNow;
        var startDate = request.StartDate.ToDateTime();
        var endDate = request.EndDate.ToDateTime();
        
        return startDate <= endDate && startDate <= now && endDate <= now;
    }
    
}
