using Asp.Versioning;
using System.Reflection;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Controllers;
using Store.Common.Utilities;

namespace Store.WebFramework.Swagger;

public static class SwaggerConfigurationExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        Assert.NotNull(services, nameof(services));

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

        services.AddSwaggerGen(options =>
        {
            var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "Edition.Api.xml");
            if (File.Exists(xmlDocPath))
                options.IncludeXmlComments(xmlDocPath, true);

            options.EnableAnnotations();
            options.CustomOperationIds(CreateOperationId);
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = SwaggerDocumentationContent.DocumentTitle,
                Description = SwaggerDocumentationContent.ApiDescription,
                Contact = new OpenApiContact
                {
                    Name = "Edition Platform",
                    Url = new Uri("https://github.com/HamidMohammadi1990")
                }
            });

            options.ExampleFilters();
            options.OperationFilter<SwaggerControllerTagsOperationFilter>();
            options.OperationFilter<ApplySummariesOperationFilter>();
            options.OperationFilter<SwaggerAudienceOperationFilter>();
            options.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "OAuth2");
            options.OperationFilter<SwaggerJsonOnlyOperationFilter>();
            options.DocumentFilter<SwaggerTagGroupsDocumentFilter>();

            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Description = "JWT Bearer — ابتدا sign-in کنید، سپس توکن را در Authorize وارد کنید.",
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri("/api/v1/account/sign-in", UriKind.Relative)
                    }
                }
            });

            options.OperationFilter<RemoveVersionParameters>();
            options.DocumentFilter<SetVersionInPaths>();

            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                var versions = methodInfo.DeclaringType
                    .GetCustomAttributes<ApiVersionAttribute>(true)
                    .SelectMany(attr => attr.Versions);

                return versions.Any(v => $"v{v}" == docName);
            });
        });
    }

    private static string CreateOperationId(ApiDescription apiDesc)
    {
        if (apiDesc.ActionDescriptor is not ControllerActionDescriptor action)
            return apiDesc.ActionDescriptor.RouteValues.TryGetValue("action", out var routeAction)
                ? routeAction
                : apiDesc.RelativePath ?? "operation";

        var isAdmin = SwaggerControllerAudience.IsAdmin(action);
        var baseId = $"{action.ControllerName}_{action.ActionName}";
        return isAdmin ? $"admin_{baseId}" : baseId;
    }

    public static IApplicationBuilder UseSwaggerAndUI(this IApplicationBuilder app)
    {
        Assert.NotNull(app, nameof(app));

        var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        if (Directory.Exists(webRoot))
        {
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = new PhysicalFileProvider(webRoot),
                DefaultFileNames = ["index.html"]
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(webRoot)
            });
        }

        app.Use(async (context, next) =>
        {
            var path = context.Request.Path;
            if (path == "/docs" || path == "/docs/")
            {
                context.Response.Redirect("/docs/index.html");
                return;
            }

            await next();
        });

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            options.DocumentTitle = $"{SwaggerDocumentationContent.DocumentTitle} — Try it out";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Edition API v1");
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableTryItOutByDefault();
            options.DisplayOperationId();
            options.DefaultModelsExpandDepth(-1);
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelRendering(ModelRendering.Example);
            options.EnablePersistAuthorization();
            options.ShowExtensions();
            options.EnableFilter();
            options.InjectStylesheet("/lib/fonts/vazirmatn/vazirmatn.css");
            options.InjectStylesheet("/docs/edition-header.css");
            options.InjectStylesheet("/swagger-ui/custom.css");
            options.InjectJavascript("/swagger-ui/edition-tag-groups.js");
            options.UseRequestInterceptor(
                "(request) => { request.headers['Accept-Language'] = window.localStorage.getItem('edition-api-language') || 'fa-IR'; return request; }");
            options.HeadContent += "<script>document.documentElement.setAttribute('dir','ltr');document.documentElement.setAttribute('lang','en');</script>";
            options.HeadContent += "<script src=\"/docs/edition-locale.js\" defer></script>";
            options.HeadContent += "<script src=\"/docs/edition-header.js\" defer></script>";
        });

        app.UseReDoc(options =>
        {
            options.RoutePrefix = "docs/read";
            options.DocumentTitle = SwaggerDocumentationContent.DocumentTitle;
            options.SpecUrl("/swagger/v1/swagger.json");
            options.ExpandResponses("200,201");
            options.RequiredPropsFirst();
            options.PathInMiddlePanel();
            options.HideHostname();
            options.SortPropsAlphabetically();
            options.NativeScrollbars();
            options.ScrollYOffset(0);
            options.InjectStylesheet("/lib/fonts/vazirmatn/vazirmatn.css");
            options.InjectStylesheet("/docs/redoc-custom.css");
            options.HeadContent += "<script src=\"/docs/edition-spec-locale.js\"></script>";
            options.HeadContent += "<script src=\"/docs/redoc-api-tester.js\" defer></script>";
            options.HeadContent += """
                <script>
                document.documentElement.setAttribute('dir', 'ltr');
                document.documentElement.setAttribute('lang', 'en');
                </script>
                """;
        });

        return app;
    }
}
