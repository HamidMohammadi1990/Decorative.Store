using System.Resources;

namespace Edition.Application.Resources;

/// <summary>Resource type for localized enum display names.</summary>
public class EnumResources
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "Edition.Application.Resources.EnumResources",
            typeof(EnumResources).Assembly);
}
