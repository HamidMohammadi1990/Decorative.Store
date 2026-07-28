using Edition.Application.Common.Caching.Enums;

namespace Edition.Application.Common.Caching.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CacheAttribute : Attribute
{
    public bool Idempotent { get; set; }
    public bool Extend { get; private init; }
    public int Duration { get; private init; }
    public CacheInstanceType CacheInstance { get; private init; }


    /// <summary>
    /// Indicate that this method automatically cache
    /// </summary>
    /// <param name="duration">Duration is based on minute</param>
    /// <param name="cacheInstance">Choose cache instance</param>
    /// <param name="extend">Indicate if cache exists, replace it with new value or not. If extend is true, it will replace with new value</param>
    public CacheAttribute(int duration, CacheInstanceType cacheInstance, bool extend = false, bool idempotent = false)
    {
        Extend = extend;
        Duration = duration * 60;
        Idempotent = idempotent;
        CacheInstance = cacheInstance;
    }
    public CacheAttribute(TimeSpan duration, CacheInstanceType cacheInstance, bool extend = false, bool idempotent = false)
    {
        switch (duration.TotalSeconds)
        {
            case <= 0:
                throw new Exception($"{nameof(duration)} must be greater than 0!");
            case > int.MaxValue:
                throw new Exception($"{nameof(duration)} must be less than {int.MaxValue} seconds!");
        }

        Extend = extend;
        Idempotent = idempotent;
        CacheInstance = cacheInstance;
        Duration = (int)duration.TotalSeconds;
    }
}