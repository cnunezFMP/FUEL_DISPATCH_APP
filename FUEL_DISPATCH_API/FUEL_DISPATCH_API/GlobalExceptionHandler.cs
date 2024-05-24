using FUEL_DISPATCH_API.Utils;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using System.Text.Json;

namespace FUEL_DISPATCH_API
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var details = new ProblemDetails
            {
                Detail = exception.Message,
                Instance = "API",
                Status = (int)exception.InnerException!.HResult,
                Title = "API Error",
                Type = "Server Error"
            };
            var response = JsonSerializer.Serialize(details);
            LoggerClass.LogError(response);
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
    }
}
