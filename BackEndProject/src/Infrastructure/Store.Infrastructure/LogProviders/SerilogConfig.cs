using Serilog;
using System.Data;
using Serilog.Sinks.MSSqlServer;
using Microsoft.Extensions.Configuration;
using Store.Infrastructure.Configurations;

namespace Store.Infrastructure.LogProviders;

public static class SerilogConfig
{
    /// <summary>
    /// Initialize Serilog with Async SQL Server Sink
    /// </summary>
    public static void UseSerilog(this SeilogConfiguration serilogConfiguration, IConfiguration configuration)
    {
        var columnOptions = new ColumnOptions
        {
            AdditionalColumns =
                [
                    new() { ColumnName = "UserId", DataType = SqlDbType.Int, AllowNull = true },
                    new() { ColumnName = "CorrelationId", DataType = SqlDbType.VarChar, DataLength = 40, AllowNull = true }
                ],
            Store = { StandardColumn.LogEvent }
        };

        //Log.Logger = new LoggerConfiguration()
        //    .Enrich.FromLogContext()
        //    .Enrich.WithMachineName()
        //    .Enrich.WithThreadId()
        //    .ReadFrom.Configuration(configuration)
        //    .WriteTo.Async(a => a.MSSqlServer(
        //        connectionString: serilogConfiguration.SQLConnectionString,
        //        sinkOptions: new MSSqlServerSinkOptions
        //        {
        //            TableName = serilogConfiguration.TableName,
        //            BatchPeriod = TimeSpan.FromSeconds(serilogConfiguration.PeriodSeconds),
        //            BatchPostingLimit = serilogConfiguration.BatchPostingLimit,
        //            AutoCreateSqlTable = serilogConfiguration.AutoCreateSqlTable
        //        },
        //        columnOptions: columnOptions),
        //        blockWhenFull: false // Zero-Blocking
        //    )
        //    .CreateLogger();
    }

    /// <summary>
    /// Safe disposal at application shutdown
    /// </summary>
    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
    }
}