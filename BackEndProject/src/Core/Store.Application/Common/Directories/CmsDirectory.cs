namespace Edition.Application.Common.Directories;

public static class CmsDirectory
{
    public static string CmsImage = "wwwroot/Uploads/Cms";

    public static string GetImageUrl(string imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
            return string.Empty;

        if (imageName.StartsWith('/') || imageName.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return imageName;

        return $"{CmsImage.Replace("wwwroot", "")}/{imageName}";
    }
}
