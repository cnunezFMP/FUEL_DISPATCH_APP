namespace FUEL_DISPATCH_API.Middlewares
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware <GlobalExceptionHandlingMiddleware>();
    }
}
