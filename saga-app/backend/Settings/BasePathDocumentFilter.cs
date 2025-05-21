using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class BasePathDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Get the base URL (including the subfolder)
        var basePath = "/api";
        swaggerDoc.Servers.Clear();
        swaggerDoc.Servers.Add(new OpenApiServer { Url = basePath });
    }
}
