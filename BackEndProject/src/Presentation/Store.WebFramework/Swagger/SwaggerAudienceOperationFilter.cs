using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Store.WebFramework.Swagger;

public sealed class SwaggerAudienceOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor descriptor)
            return;

        var tag = SwaggerTagDescriptor.Resolve(descriptor);
        var label = tag.Audience;

        if (string.IsNullOrWhiteSpace(operation.Summary))
            operation.Summary = SwaggerEndpointNaming.FromAction(descriptor);

        operation.Summary = PrependAudience(operation.Summary, label);
        operation.Description = PrependAudience(operation.Description, label);

        operation.Extensions ??= new Dictionary<string, IOpenApiExtension>();
        operation.Extensions["x-audience"] = new JsonNodeExtension(JsonValue.Create(tag.Audience));
    }

    private static string PrependAudience(string? text, string label)
    {
        var badge = $"[{label}]";
        if (string.IsNullOrWhiteSpace(text))
            return badge;

        if (text.StartsWith('['))
            return text;

        return $"{badge} {text}";
    }
}
