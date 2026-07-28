namespace Edition.Application.Common.Directories;

public class OrderItemDirectory
{
    public static string OrderImage = "wwwroot/Uploads/Orders";
    public static string GetImageUrl(string imageName)
    {
        return $"{OrderImage.Replace("wwwroot", "")}/{imageName}";
    }
}