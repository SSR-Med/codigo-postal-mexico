namespace Api.Middlewares.Validators
{
    public class XRequestIdValidator
    {
        public static void Ensure(HttpContext context)
        {
            var requestId = context.Request.Headers.TryGetValue("X-Request-ID", out var rid) && !string.IsNullOrWhiteSpace(rid)
                ? rid.ToString()
                : Guid.NewGuid().ToString();

            context.Items["X-Request-ID"] = requestId;
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Request-ID"] = requestId;
                return Task.CompletedTask;
            });
        }
    }
}