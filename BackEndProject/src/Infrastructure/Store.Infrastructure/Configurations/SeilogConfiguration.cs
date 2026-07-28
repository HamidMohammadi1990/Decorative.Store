namespace Store.Infrastructure.Configurations;

public class SeilogConfiguration
{
    public string SQLConnectionString { get; set; } = default!;
    public string TableName { get; set; } = default!;
    public bool AutoCreateSqlTable { get; set; }
    public int BatchPostingLimit { get; set; }
    public int PeriodSeconds { get; set; }
}