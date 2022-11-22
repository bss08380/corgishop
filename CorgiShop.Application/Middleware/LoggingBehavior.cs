using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorgiShop.Application.Middleware;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{typeof(TRequest).Name}: Request Received");
        _logger.LogDebug($"{typeof(TRequest).Name} Data: {JsonSerializer.Serialize(request)}");

        var response = await next();

        _logger.LogInformation($"{typeof(TRequest).Name}: Response Completed");
        _logger.LogDebug($"{typeof(TRequest).Name} Data: {JsonSerializer.Serialize(response)}");

        return response;
    }
}
