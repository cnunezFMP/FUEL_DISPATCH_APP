using FUEL_DISPATCH_API.Middlewares;

namespace FUEL_DISPATCH_API.Builders
{
    public static class UnauthorizedMiddlewareExtensions
    {
        public static IApplicationBuilder UseUnauthorizedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UnauthorizedMiddleware>();
        }
    }
}
