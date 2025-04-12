using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Implements an operation filter to apply security requirements to Swagger operations based on authorization attributes.
/// </summary>
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies security requirements to the specified Swagger operation based on authorization attributes.
    /// </summary>
    /// <param name="operation">The Swagger operation to apply security requirements to.</param>
    /// <param name="context">The context for the Swagger operation filter.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authAttributes = context.MethodInfo.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
            .Distinct();

        if (authAttributes.Any())
        {
            var requirements = new List<OpenApiSecurityRequirement>();
            foreach (var authAttribute in authAttributes)
            {
                var requirement = new OpenApiSecurityRequirement
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
                        new[] { authAttribute.Policy ?? "" }
                    }
                };
                requirements.Add(requirement);
            }

            operation.Security = requirements;
        }
    }
}
