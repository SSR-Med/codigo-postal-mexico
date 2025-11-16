namespace Api.Middlewares.Validators
{
    public class XChannelIdValidator
    {
        public static async Task<bool> ValidateAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-Channel-Id", out var channelHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Missing required header: X-Channel-Id");
                return false;
            }

            var value = channelHeader.ToString();
            context.Items["X-Channel-Id"] = value;
            return true;
        }
    }
}