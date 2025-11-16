namespace Api.Filters
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class CorrelationIdHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Correlation-ID",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema { Type = "string", Format = "uuid" },
                Description = "ID para correlacionar eventos entre servicios (UUID)"
            });
        }
    }
}