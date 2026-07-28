using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Store.WebFramework.Swagger;

public sealed class SwaggerControllerTagsOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor descriptor)
            return;

        var tag = SwaggerTagDescriptor.Resolve(descriptor);

        context.Document.Tags ??= new HashSet<OpenApiTag>();
        if (!context.Document.Tags.Any(x => string.Equals(x.Name, tag.TagName, StringComparison.Ordinal)))
        {
            context.Document.Tags.Add(new OpenApiTag
            {
                Name = tag.TagName,
                Description = $"{tag.CategoryDisplayName} · {tag.Title}"
            });
        }

        operation.Tags ??= new HashSet<OpenApiTagReference>();
        operation.Tags.Clear();
        operation.Tags.Add(new OpenApiTagReference(tag.TagName, context.Document));
    }
}