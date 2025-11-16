namespace Api.Filters
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Microsoft.AspNetCore.Authorization;

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionAttributes = context.MethodInfo.GetCustomAttributes(true);
            var actionAuthorizeAttribute = actionAttributes.FirstOrDefault(attr => attr.GetType() == typeof(AuthorizeAttribute)) as AuthorizeAttribute;
            var hasAuthorizeAttribute = actionAuthorizeAttribute is not null;
            if (hasAuthorizeAttribute)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new List<string>()
                        }
                    }
                };
            }
            else
            {
                operation.Security = null;
            }
        }
    }
}
