
namespace FUEL_DISPATCH_API.Middlewares
{
    public class UnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;
        public UnauthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.StatusCode is StatusCodes.Status401Unauthorized ||
                               context.Response.StatusCode is StatusCodes.Status403Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User not authorized. ");
                return;
            }
            await _next(context);
        }
    }
}
