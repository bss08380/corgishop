using CorgiShop.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

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
        catch (DbUpdateException ex)
        {
            LogError(ex);
            await HandleExceptionAsync(httpContext, DetailedException.FromDatabaseUpdateError(ex));
        }
        catch (DetailedException ex)
        {
            LogError(ex);
            await HandleExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            LogError(ex);
            await HandleExceptionAsync(httpContext, DetailedException.FromUnknownError(ex));
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, DetailedException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exception.Details.StatusCode;
        await context.Response.WriteAsync(exception.Details.ToString());
    }

    private void LogError(Exception ex, string? msg = null) => _logger.LogError(msg ?? $"Exception Middleware: {ex.Message}", ex);
}
