using Serilog;
using Edition.Application.Contracts.Localization;
using Store.Infrastructure.LogProviders;
using Store.Infrastructure.Persistence.Contracts;

namespace Store.Api.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Seeds required reference data on startup: Language (fa-IR, en-US) then catalog and blog categories.
    /// </summary>
    public static async Task SeedApplicationDataAsync(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();

        var languageBootstrap = scope.ServiceProvider.GetRequiredService<ILanguageBootstrapService>();
        await languageBootstrap.EnsureLanguagesReadyAsync(cancellationToken);

        var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
        await seedService.SeedCatalogAsync(cancellationToken);
    }
    /// <summary>
    /// Close And Flush Serilog When Application Stopping
    /// </summary>
    /// <param name="app"></param>
    public static void CloseSerilogWhenApplicationStopping(this WebApplication app)
    {
        app.Lifetime.ApplicationStopping.Register(SerilogConfig.CloseAndFlush);
    }

    /// <summary>
    /// Handling Exceptions When First Run Program
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public async static Task CustomRunAsync(this WebApplication app)
    {
        try
        {
            Log.Information("Application Starting Up");

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application failed to start");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}