using System.Resources;

namespace Store.Common.Resources;

/// <summary>Resource type for localized enum display names.</summary>
public class EnumResources
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "Store.Common.Resources.EnumResources",
            typeof(EnumResources).Assembly);
}
