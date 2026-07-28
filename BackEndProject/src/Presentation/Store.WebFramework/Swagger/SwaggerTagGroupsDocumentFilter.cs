using System.Text.Json;
using System.Text.Json.Nodes;
using Store.Common.Enums;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;
using Store.Common.Extensions;

namespace Store.WebFramework.Swagger;

public sealed class SwaggerTagGroupsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var descriptors = context.ApiDescriptions
            .Select(x => x.ActionDescriptor as ControllerActionDescriptor)
            .Where(x => x is not null)
            .Select(x => SwaggerTagDescriptor.Resolve(x!))
            .GroupBy(x => x.TagName, StringComparer.Ordinal)
            .Select(x => x.First())
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Title, StringComparer.Ordinal)
            .ToList();

        swaggerDoc.Tags = new HashSet<OpenApiTag>(
            descriptors.Select(x => new OpenApiTag
            {
                Name = x.TagName,
                Description = $"{x.Category.GetEnglishDisplayName()} · {x.Title}"
            }));

        var tagGroups = BuildTagGroups(descriptors);
        swaggerDoc.AddExtension(
            "x-tagGroups",
            new JsonNodeExtension(JsonSerializer.SerializeToNode(tagGroups)!));
    }

    private static IReadOnlyList<object> BuildTagGroups(IReadOnlyList<SwaggerTagDescriptor> descriptors)
    {
        return descriptors
            .GroupBy(x => x.Category)
            .OrderBy(x => (int)x.Key)
            .Select(group => new
            {
                name = group.Key.GetEnglishDisplayName(),
                tags = group
                    .OrderBy(x => x.Title, StringComparer.Ordinal)
                    .Select(x => x.TagName)
                    .Distinct(StringComparer.Ordinal)
                    .ToList()
            })
            .Where(group => group.tags.Count > 0)
            .Cast<object>()
            .ToList();
    }
}