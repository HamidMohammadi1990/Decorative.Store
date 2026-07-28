using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Store.WebFramework.Swagger;

public class RemoveVersionParameters : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Remove version parameter from all Operations
        if (operation.Parameters is null)
            return;

        var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
        if (versionParameter is not null)
            operation.Parameters.Remove(versionParameter);
    }
}