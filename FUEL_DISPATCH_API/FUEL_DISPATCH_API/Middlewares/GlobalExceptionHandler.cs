using FUEL_DISPATCH_API.Utils;
using FUEL_DISPATCH_API.Utils.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FUEL_DISPATCH_API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problemDetails = new();
            var exType = exception.GetType();
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            CheckException(httpContext, exception, out problemDetails, exType, out httpStatusCode);
            var response = JsonSerializer.Serialize(problemDetails);
            LoggerClass.LogError(response);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)httpStatusCode;
            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
        private static void CheckException(HttpContext httpContext, Exception exception, out ProblemDetails problemDetails, Type exType, out HttpStatusCode httpStatusCode)
        {
            if (exType == typeof(BadRequestException))
            {
                var details = new ProblemDetails
                {
                    Detail = exception.Message,
                    Instance = httpContext.ToString(),
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Type = exType.ToString()
                };
                httpStatusCode = HttpStatusCode.BadRequest;
                problemDetails = details;
            }
            else if (exType == typeof(UnauthorizedException))
            {
                var details = new ProblemDetails
                {
                    Detail = exception.Message,
                    Instance = httpContext.ToString(),
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Auth Error",
                    Type = exType.ToString()
                };
                httpStatusCode = HttpStatusCode.Unauthorized;
                problemDetails = details;

            }
            else if (exType == typeof(NotFoundException))
            {
                var details = new ProblemDetails
                {
                    Detail = exception.Message,
                    Instance = httpContext.ToString(),
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found Error",
                    Type = exType.ToString()
                };
                httpStatusCode = HttpStatusCode.NotFound;
                problemDetails = details;

            }
            else if (exType == typeof(SystemNetMailSmtpException))
            {
                var details = new ProblemDetails
                {
                    Detail = exception.Message,
                    Instance = httpContext.ToString(),
                    Status = StatusCodes.Status503ServiceUnavailable,
                    Title = "Problem ocurred with the SMTP Server.",
                    Type = exType.ToString()
                };
                httpStatusCode = HttpStatusCode.InternalServerError;
                problemDetails = details;
            }
            else
            {
                var details = new ProblemDetails
                {
                    Detail = "An unexpected error has occurred. ",
                    Instance = httpContext.ToString(),
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Something went wrong. ",
                    Type = exType.ToString()
                };
                httpStatusCode = HttpStatusCode.InternalServerError;
                problemDetails = details;
            }
        }
    }
}
