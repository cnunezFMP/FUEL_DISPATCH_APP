using FUEL_DISPATCH_API.Utils;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
namespace FUEL_DISPATCH_API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            ResultPattern<ProblemDetails> problemDetails = new();
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

        private static void CheckException(
            HttpContext httpContext,
            Exception exception,
            out ResultPattern<ProblemDetails> problemDetails,
            Type exType,
            out HttpStatusCode httpStatusCode)
        {
            if (exType == typeof(BadRequestException))
            {
                var details = new ResultPattern<ProblemDetails>
                {
                    Message = exception.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false
                };
                httpStatusCode = HttpStatusCode.BadRequest;
                problemDetails = details;
            }
            else if (exType == typeof(UnauthorizedException))
            {
                var details = new ResultPattern<ProblemDetails>
                {
                    Message = exception.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false

                };
                httpStatusCode = HttpStatusCode.Unauthorized;
                problemDetails = details;

            }
            else if (exType == typeof(NotFoundException))
            {
                var details = new ResultPattern<ProblemDetails>
                {
                    Message = exception.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false
                };
                httpStatusCode = HttpStatusCode.NotFound;
                problemDetails = details;

            }
            else
            {
                var details = new ResultPattern<ProblemDetails>
                {
                    Message = exception.Message,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IsSuccess = false
                };
                httpStatusCode = HttpStatusCode.InternalServerError;
                problemDetails = details;
            }
        }
    }
}
