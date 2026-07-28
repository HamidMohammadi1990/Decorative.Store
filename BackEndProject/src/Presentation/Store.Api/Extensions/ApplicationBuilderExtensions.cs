using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Store.Api.Middlewares;
using Store.Common.Models;

namespace Store.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UserCustomeForwardedHeaders(this IApplicationBuilder app)
    {
        var settings = app.ApplicationServices.GetRequiredService<ForwardedHeadersSettings>();
        var options = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };

        foreach (var proxy in settings.KnownProxies)
        {
            if (IPAddress.TryParse(proxy, out var ipAddress))
                options.KnownProxies.Add(ipAddress);
        }

        app.UseForwardedHeaders(options);
    }

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}