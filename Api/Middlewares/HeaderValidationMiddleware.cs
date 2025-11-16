namespace Api.Middlewares
{
    using Api.Middlewares.Validators;

    public class HeaderValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (!await XChannelIdValidator.ValidateAsync(context)) return;

            XRequestIdValidator.Ensure(context);
            XCorrelationIdValidator.Ensure(context);

            await _next(context);
        }
    }
}