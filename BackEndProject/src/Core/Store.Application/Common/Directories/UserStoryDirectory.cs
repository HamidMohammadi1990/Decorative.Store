namespace Edition.Application.Common.Directories;

public static class UserStoryDirectory
{
    public const string StoryMedia = "wwwroot/Uploads/Stories";
    public const string PublicMediaPrefix = "/Uploads/Stories/";

    public static string ToPublicUrl(string fileName)
        => $"{PublicMediaPrefix}{fileName}";
}
