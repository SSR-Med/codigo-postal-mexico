namespace Api.Middlewares.Validators
{
    public class XCorrelationIdValidator
    {
        public static void Ensure(HttpContext context)
        {
            var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var cid) && !string.IsNullOrWhiteSpace(cid)
                ? cid.ToString()
                : Guid.NewGuid().ToString();

            context.Items["X-Correlation-ID"] = correlationId;
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Correlation-ID"] = correlationId;
                return Task.CompletedTask;
            });
        }
    }
}