using Serilog;
using Store.Infrastructure.LogProviders;

namespace Store.Api.Extensions;

public static class WebApplicationExtensions
{
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