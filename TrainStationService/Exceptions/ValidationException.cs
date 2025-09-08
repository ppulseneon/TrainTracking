namespace TrainStationService.Exceptions;

public class ValidationException(IReadOnlyDictionary<string, string[]> errors)
    : Exception("Одна или несколько ошибок валидации")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;

    public ValidationException(string propertyName, string errorMessage)
        : this(new Dictionary<string, string[]> { { propertyName, [errorMessage] } })
    {
    }

    public ValidationException(string errorMessage)
        : this("ValidationError", errorMessage)
    {
    }
}
