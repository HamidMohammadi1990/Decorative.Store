using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Store.WebFramework.Swagger;

public sealed class SwaggerJsonOnlyOperationFilter : IOperationFilter
{
    private const string Json = "application/json";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        NormalizeRequestBody(operation);
        NormalizeResponses(operation);
    }

    private static void NormalizeRequestBody(OpenApiOperation operation)
    {
        if (operation.RequestBody?.Content is not { Count: > 0 } content)
            return;

        var media = PickJsonMediaType(content);
        content.Clear();
        content[Json] = media;
    }

    private static void NormalizeResponses(OpenApiOperation operation)
    {
        foreach (var response in operation.Responses.Values)
        {
            if (response.Content is not { Count: > 0 } content)
                continue;

            var media = PickJsonMediaType(content);
            content.Clear();
            content[Json] = media;
        }
    }

    private static OpenApiMediaType PickJsonMediaType(IDictionary<string, OpenApiMediaType> content)
    {
        if (content.TryGetValue(Json, out var json))
            return json;

        foreach (var key in new[] { "text/json", "application/*+json" })
        {
            if (content.TryGetValue(key, out var media))
                return media;
        }

        return content.Values.First();
    }
}
