using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProfileCompletionSetting : BaseEntity
{
    public string ConfigJson { get; private set; } = default!;

    public static ProfileCompletionSetting Create(string configJson)
        => new() { ConfigJson = configJson };

    public void UpdateConfig(string configJson)
    {
        ConfigJson = configJson;
    }
}
