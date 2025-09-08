using Grpc.Core;
using Grpc.Core.Interceptors;
using TrainStationService.Exceptions;

namespace TrainStationService.Interceptors;

public class ValidationInterceptor(ILogger<ValidationInterceptor> logger) : Interceptor
{
    private const string InternalErrorMessage = "Внутренняя ошибка";
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException validationEx)
        {
            logger.LogWarning("Ошибка валидации {Method}: {Errors}",
                context.Method,
                string.Join("; ", validationEx.Errors.SelectMany(e => e.Value)));

            var errorMessage = string.Join("; ", validationEx.Errors.SelectMany(e => e.Value));
            
            var metadata = new Metadata();
            foreach (var error in validationEx.Errors)
            {
                metadata.Add($"validation-{error.Key.ToLowerInvariant()}", 
                    string.Join(", ", error.Value));
            }

            throw new RpcException(
                new Status(StatusCode.InvalidArgument, $"Ошибка валидации: {errorMessage}"),
                metadata);
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
        catch (ValidationException validationEx)
        {
            logger.LogWarning("Ошибка валидации в потоковом запросе {Method}: {Errors}",
                context.Method,
                string.Join("; ", validationEx.Errors.SelectMany(e => e.Value)));

            var errorMessage = string.Join("; ", validationEx.Errors.SelectMany(e => e.Value));
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Ошибка валидации: {errorMessage}"));
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            logger.LogError(ex, "Необработанное исключение в потоковом методе {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.Internal, InternalErrorMessage));
        }
    }
}
