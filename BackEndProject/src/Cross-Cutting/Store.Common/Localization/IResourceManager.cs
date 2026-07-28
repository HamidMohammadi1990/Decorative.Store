namespace Store.Common.Localization;

public interface IResourceManager
{
    string GetString(string key);
    string GetString(string key, params object[] formatArgs);
    string ResolveMessage(string keyOrMessage);
}
