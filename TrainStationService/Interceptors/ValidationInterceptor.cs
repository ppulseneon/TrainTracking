using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace TrainStationService.Interceptors;

public class ValidationInterceptor(ILogger<ValidationInterceptor> logger, IServiceProvider serviceProvider) : Interceptor
{
    private const string InternalErrorMessage = "Internal error";
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await ValidateRequestAsync(request, context.CancellationToken);
            
            return await continuation(request, context);
        }
        catch (TrainStationService.Exceptions.ValidationException validationEx)
        {
            logger.LogWarning("Ошибка валидации {Method}: {Errors}",
                context.Method,
                string.Join("; ", validationEx.Errors.SelectMany(e => e.Value)));

            var errorMessage = string.Join("; ", validationEx.Errors.SelectMany(e => e.Value));
            
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, $"Validation error: {errorMessage}"));
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning("Некорректный аргумент в запросе {Method}: {Message}",
                context.Method, argEx.Message);

            var metadata = new Metadata
            {
                { "error-type", "ArgumentException" },
                { "error-details", argEx.Message }
            };

            throw new RpcException(
                new Status(StatusCode.InvalidArgument, $"Некорректные входные данные: {argEx.Message}"),
                metadata);
        }
        catch (TimeoutException timeoutEx)
        {
            logger.LogError(timeoutEx, "Было превышено время ожидания {Method}", context.Method);

            throw new RpcException(new Status(StatusCode.DeadlineExceeded, "Превышено время ожидания"));
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            logger.LogError(ex, "Необработанное исключение {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.Internal, InternalErrorMessage));
        }
    }

    /// <summary>
    /// Перехват серверных потоков
    /// </summary>
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (TrainStationService.Exceptions.ValidationException validationEx)
        {
            logger.LogWarning("Ошибка валидации в потоковом запросе {Method}: {Errors}",
                context.Method,
                string.Join("; ", validationEx.Errors.SelectMany(e => e.Value)));

            var errorMessage = string.Join("; ", validationEx.Errors.SelectMany(e => e.Value));
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Validation error: {errorMessage}"));
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            logger.LogError(ex, "Необработанное исключение в потоковом методе {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.Internal, InternalErrorMessage));
        }
    }
    
    private async Task ValidateRequestAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        var validator = serviceProvider.GetService<IValidator<TRequest>>();
        if (validator == null) return;

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            
            throw new TrainStationService.Exceptions.ValidationException(errors);
        }
    }
}
