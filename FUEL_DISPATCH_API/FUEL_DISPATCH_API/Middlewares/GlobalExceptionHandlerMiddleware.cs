using FUEL_DISPATCH_API.Utils;
using FUEL_DISPATCH_API.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
namespace FUEL_DISPATCH_API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            var stackTrace = string.Empty;
            string message = "";

            var exType = ex.GetType();

            if (exType == typeof(BadRequestException))
            {
                message = ex.Message;
                status = HttpStatusCode.BadRequest;
                stackTrace = ex.StackTrace;
            }
            else if (exType == typeof(NotFoundException))
            {
                message = ex.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = ex.StackTrace;
            }
            else if (exType == typeof(UnauthorizedException))
            {
                message = ex.Message;
                status = HttpStatusCode.Unauthorized;
                stackTrace = ex.StackTrace;
            }
            else
            {
                message = ex.Message;
                status = HttpStatusCode.InternalServerError;
                stackTrace = ex.StackTrace;
            }
            var reponse = new ProblemDetails
            {
                Title = message,
                Detail = stackTrace,
                Status = (int)status,
                Type = exType.ToString(),
            };
            var responseObj = JsonSerializer.Serialize(reponse);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            LoggerClass.LogError(responseObj);
        }
    }
}

