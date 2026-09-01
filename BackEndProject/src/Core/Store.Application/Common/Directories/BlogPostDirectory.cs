namespace Edition.Application.Common.Directories;

public class BlogPostDirectory
{
    public static string BlogPostImage = "wwwroot/Uploads/BlogPosts";

    public static string GetImageUrl(string imageName)
        => $"{BlogPostImage.Replace("wwwroot", "")}/{imageName}";
}
