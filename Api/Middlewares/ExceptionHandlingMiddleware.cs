namespace Api.Middlewares
{
    using System.Net;
    using Newtonsoft.Json;

    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        #region Parameters
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        #endregion


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleGlobalExceptionAsync(context, ex);
            }
        }

        private Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, $"An unexpected error occurred: {exception.Message}");
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            var result = JsonConvert.SerializeObject(new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                title = "An error occurred while processing your request.",
                status = context.Response.StatusCode,
                detail = exception.Message
            });

            return context.Response.WriteAsync(result);
        }
    }
}