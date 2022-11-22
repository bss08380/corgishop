using CorgiShop.Common.Model;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CorgiShop.Api.Infrastructure;

public static class MiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
