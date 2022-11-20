using CorgiShop.Common.Model;
using System.Net;

namespace CorgiShop.Api.Infrastructure;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        /*
         * Catch specific exception types here as needed
         */
        catch (Exception ex)
        {
            LogError(ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Details = exception.Message ?? "Unknown error"
        }.ToString());
    }

    private void LogError(Exception ex, string? msg = null) => _logger.LogError(msg ?? $"Exception Middleware: {ex.Message}", ex);
}
