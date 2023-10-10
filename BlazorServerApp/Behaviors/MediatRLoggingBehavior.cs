using BlazorServerApp.Injectables;
using BlazorServerApp.Logger;
using MediatR;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BlazorServerApp.Behaviors;

public interface IMediatRLoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>, ITransientService
    where TRequest : MediatR.IRequest<TResponse>
{ }

public class MediatRLoggingBehavior<TRequest, TResponse>
    : IMediatRLoggingBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
{
    private readonly IApiLogger _logger;

    public MediatRLoggingBehavior(IApiLogger logger)
    {
        _logger = logger;
    }

    public Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        string uniqueId = Guid.NewGuid().ToString();
        string requestMessage = JsonSerializer.Serialize(request,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

        _logger.LogInformation($"Begin Request Id:{uniqueId}, " +
            $"request name:{requestName},\nRequest={requestMessage}");

        var timer = new Stopwatch();
        timer.Start();
        var response = next();
        timer.Stop();

        string responseMessage = JsonSerializer.Serialize(response.Result,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

        _logger.LogInformation($"End Request Id:{uniqueId}, " +
            $"request name:{requestName},\nResponse={responseMessage}" +
            $"\ntotal elapsed time: {timer.ElapsedMilliseconds}");

        return response;
    }
}