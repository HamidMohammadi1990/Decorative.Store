using System.Resources;

namespace Store.Common.Resources;

public class ControllerCategoryResources
{
    private static ResourceManager? _resourceManager;

    public static ResourceManager ResourceManager =>
        _resourceManager ??= new ResourceManager(
            "Edition.Common.Resources.ControllerCategoryResources",
            typeof(ControllerCategoryResources).Assembly);
}
