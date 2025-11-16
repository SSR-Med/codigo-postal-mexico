namespace Api.Filters
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class RequestIdHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Request-ID",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema { Type = "string", Format = "uuid" },
                Description = "Identificador único de la solicitud (UUID generado por el cliente)"
            });
        }
    }
}