using Pluralize.NET;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;
using Store.Common.Extensions;

namespace Store.WebFramework.Swagger;

public class ApplySummariesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
            return;

        operation.Parameters ??= [];

        var pluralizer = new Pluralizer();
        var actionName = controllerActionDescriptor.ActionName;
        var httpMethod = context.ApiDescription.HttpMethod ?? "GET";
        var singularizeName = pluralizer.Singularize(controllerActionDescriptor.ControllerName);
        var pluralizeName = pluralizer.Pluralize(singularizeName);
        var parameterCount = operation.Parameters.Count(p => p.Name is not "version" and not "api-version");

        if (IsGetAllAction())
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Returns all {pluralizeName}";
        }
        else if (IsActionName("Post", "Create"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Creates a {singularizeName}";

            TrySetFirstParameterDescription($"A {singularizeName} representation");
        }
        else if (IsActionName("Read", "Get"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Retrieves a {singularizeName} by unique id";

            TrySetFirstParameterDescription($"a unique id for the {singularizeName}");
        }
        else if (IsActionName("Put", "Edit", "Update"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Updates a {singularizeName} by unique id";

            TrySetFirstParameterDescription($"A {singularizeName} representation");
        }
        else if (IsActionName("Delete", "Remove"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Deletes a {singularizeName} by unique id";

            TrySetFirstParameterDescription($"A unique id for the {singularizeName}");
        }

        if (!operation.Summary.HasValue())
            operation.Summary = SwaggerEndpointNaming.FromAction(controllerActionDescriptor);

        #region Local Functions
        void TrySetFirstParameterDescription(string description)
        {
            var parameter = operation.Parameters.FirstOrDefault(p => p.Name is not "version" and not "api-version");
            if (parameter is null || parameter.Description.HasValue())
                return;

            parameter.Description = description;
        }

        bool IsGetAllAction()
        {
            foreach (var name in new[] { "Get", "Read", "Select" })
            {
                if (actionName.Equals($"{name}All", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}{pluralizeName}", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}All{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}All{pluralizeName}", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (actionName.Equals(name, StringComparison.OrdinalIgnoreCase)
                    && parameterCount == 0
                    && httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsActionName(params string[] names)
        {
            foreach (var name in names)
            {
                if (actionName.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
