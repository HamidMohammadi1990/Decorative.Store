namespace Edition.Application.Common.Directories;

public static class RoomTypeDirectory
{
    public static string RoomTypeImage = "wwwroot/Uploads/RoomTypes";

    public static string GetImageUrl(string imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
            return string.Empty;

        if (imageName.StartsWith('/') || imageName.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return imageName;

        return $"{RoomTypeImage.Replace("wwwroot", "")}/{imageName}";
    }
}
