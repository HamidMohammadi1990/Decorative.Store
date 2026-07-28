using System.Diagnostics;
using Edition.Application.Common.Caching.Enums;
using Edition.Application.Common.Caching.Abstractions;

namespace Store.Infrastructure.CacheProviders;

public class DatabaseSelector : IDatabaseSelector
{
    [DebuggerStepThrough]
    public int Select(CacheInstanceType cacheInstance) => cacheInstance switch
    {
        CacheInstanceType.Default or CacheInstanceType.AppSettings or CacheInstanceType.Default => 0,
        _ => 5,
    };
}