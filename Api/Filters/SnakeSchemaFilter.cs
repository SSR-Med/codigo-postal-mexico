namespace Api.Filters
{
    using System.Text.RegularExpressions;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class SnakeSchemaFilter : IOperationFilter, ISchemaFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                foreach (var param in operation.Parameters)
                {
                    param.Name = ToSnakeCase(param.Name);
                }
            }

            if (operation.RequestBody?.Content != null)
            {
                foreach (var content in operation.RequestBody.Content)
                {
                    var schema = content.Value.Schema;
                    if (schema?.Properties != null)
                    {
                        var newProps = schema.Properties.ToDictionary(
                            kvp => ToSnakeCase(kvp.Key),
                            kvp => kvp.Value
                        );
                        schema.Properties.Clear();
                        foreach (var item in newProps)
                            schema.Properties.Add(item.Key, item.Value);
                    }
                }
            }
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null) return;

            var newProps = schema.Properties
                .ToDictionary(
                    kvp => ToSnakeCase(kvp.Key),
                    kvp => kvp.Value
                );

            schema.Properties.Clear();
            foreach (var item in newProps)
                schema.Properties.Add(item.Key, item.Value);
        }

        private static string ToSnakeCase(string input) =>  Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}