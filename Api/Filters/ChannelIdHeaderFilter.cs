namespace Api.Filters
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ChannelIdHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Channel-ID",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema 
                { 
                    Type = "string", 
                    Default = new Microsoft.OpenApi.Any.OpenApiString("GD") 
                },
                Description = "Canal desde donde se origina la solicitud (Ej: CRM, PLD, GD)"
            });
        }
    }
}