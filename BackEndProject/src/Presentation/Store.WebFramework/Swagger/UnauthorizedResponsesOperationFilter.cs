using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Store.WebFramework.Swagger;

public class UnauthorizedResponsesOperationFilter
    (bool includeUnauthorizedAndForbiddenResponses, string schemeName = "Bearer")
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors;
        var metadta = context.ApiDescription.ActionDescriptor.EndpointMetadata;

        var hasAnonymous = filters.Any(p => p.Filter is AllowAnonymousFilter) || metadta.Any(p => p is AllowAnonymousAttribute);
        if (hasAnonymous) return;

        var hasAuthorize = filters.Any(p => p.Filter is AuthorizeFilter) || metadta.Any(p => p is AuthorizeAttribute);
        if (!hasAuthorize) return;

        if (includeUnauthorizedAndForbiddenResponses)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
        }

        operation.Security ??= [];

        operation.Security.Add(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference(schemeName),
                    new List<string>()
                }
            });
    }
}