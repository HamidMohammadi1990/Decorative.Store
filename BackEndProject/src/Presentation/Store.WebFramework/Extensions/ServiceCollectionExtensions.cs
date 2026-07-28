using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Store.Common.Models;

namespace Store.WebFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });
    }

    public static void AddCustomResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });
    }

    public static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>() ?? new CorsSettings();
        services.AddSingleton(corsSettings);

        services.AddCors(options => options.AddPolicy("CustomCors", builder =>
        {
            if (corsSettings.AllowedOrigins.Length == 0)
            {
                builder.SetIsOriginAllowed(_ => false);
                return;
            }

            builder.WithOrigins(corsSettings.AllowedOrigins)
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));
    }
}