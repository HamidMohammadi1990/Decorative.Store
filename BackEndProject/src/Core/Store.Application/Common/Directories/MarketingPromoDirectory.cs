namespace Edition.Application.Common.Directories;

public static class MarketingPromoDirectory
{
    public static string MarketingPromoImage = "wwwroot/Uploads/MarketingPromos";

    public static string GetImageUrl(string imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
            return string.Empty;

        if (imageName.StartsWith('/') || imageName.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return imageName;

        return $"{MarketingPromoImage.Replace("wwwroot", "")}/{imageName}";
    }
}
