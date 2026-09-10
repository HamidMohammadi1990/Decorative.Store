namespace Edition.Application.Common.Directories;

public static class UserDirectory
{
    public static string UserImage = "wwwroot/Uploads/Users";

    public static string GetImageUrl(string? imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
            return string.Empty;

        if (imageName.StartsWith('/') || imageName.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return imageName;

        return $"{UserImage.Replace("wwwroot", "")}/{imageName}";
    }
}
