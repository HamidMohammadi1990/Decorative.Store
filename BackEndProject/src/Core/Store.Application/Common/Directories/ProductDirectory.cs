namespace Edition.Application.Common.Directories;

public class ProductDirectory
{
    public static string ProductImage = "wwwroot/Uploads/Products";
    public static string GetImageUrl(string imageName)
    {
        return $"{ProductImage.Replace("wwwroot", "")}/{imageName}";
    }
}