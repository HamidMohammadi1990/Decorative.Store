using System.Globalization;
using System.Resources;

namespace Store.Common.Localization;

public sealed class EditionResourceManager : IResourceManager
{
    private static readonly ResourceManager ResourceManager = new(
        "Store.Common.Resources.Messages",
        typeof(EditionResourceManager).Assembly);

    public string GetString(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        return ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
    }

    public string GetString(string key, params object[] formatArgs)
    {
        var template = GetString(key);
        return formatArgs.Length == 0 ? template : string.Format(CultureInfo.CurrentUICulture, template, formatArgs);
    }

    public string ResolveMessage(string keyOrMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(keyOrMessage);
        var localized = ResourceManager.GetString(keyOrMessage, CultureInfo.CurrentUICulture);
        return localized ?? keyOrMessage;
    }
}
