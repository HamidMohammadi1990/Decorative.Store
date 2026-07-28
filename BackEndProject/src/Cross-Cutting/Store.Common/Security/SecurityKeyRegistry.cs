using Store.Common.Models;

namespace Store.Common.Security;

public static class SecurityKeyRegistry
{
    private static IReadOnlyDictionary<string, string> _entityKeys = new Dictionary<string, string>(StringComparer.Ordinal);
    private static string _generalKey = string.Empty;

    public static void Initialize(SecuritySettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.GeneralKey);

        if (settings.EntityKeys.Count == 0)
            throw new InvalidOperationException("SecuritySettings:EntityKeys must contain encryption keys.");

        _generalKey = settings.GeneralKey;
        _entityKeys = new Dictionary<string, string>(settings.EntityKeys, StringComparer.Ordinal);
    }

    public static string GeneralKey
    {
        get
        {
            EnsureInitialized();
            return _generalKey;
        }
    }

    public static string Get(string entityName)
    {
        EnsureInitialized();

        if (_entityKeys.TryGetValue(entityName, out var key) && !string.IsNullOrWhiteSpace(key))
            return key;

        throw new InvalidOperationException($"Security key for entity '{entityName}' is not configured.");
    }

    private static void EnsureInitialized()
    {
        if (string.IsNullOrWhiteSpace(_generalKey))
            throw new InvalidOperationException("SecurityKeyRegistry has not been initialized.");
    }
}
