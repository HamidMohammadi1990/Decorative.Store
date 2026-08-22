using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Store.Common.Models;

namespace Edition.Application.Features.UserStories.Commands;

public record UploadUserStoryMediaRequest(IFormFile File) : IRequest<OperationResult<UploadUserStoryMediaResponse>>;

public record UploadUserStoryMediaResponse
{
    public string MediaPath { get; init; } = default!;
    public Store.Domain.Enums.StoryMediaType MediaType { get; init; }
}

public class UploadUserStoryMediaHandler(ILocalFileService localFileService)
    : IRequestHandler<UploadUserStoryMediaRequest, OperationResult<UploadUserStoryMediaResponse>>
{
    private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp",
    };

    private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4", ".webm", ".mov",
    };

    public async Task<OperationResult<UploadUserStoryMediaResponse>> Handle(
        UploadUserStoryMediaRequest request,
        CancellationToken cancellationToken)
    {
        var file = request.File;
        if (file is null || file.Length == 0)
            return ErrorModel.Create("InvalidFile");

        if (file.Length > 8 * 1024 * 1024)
            return ErrorModel.Create("FileTooLarge");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        Store.Domain.Enums.StoryMediaType mediaType;
        if (ImageExtensions.Contains(extension))
            mediaType = Store.Domain.Enums.StoryMediaType.Image;
        else if (VideoExtensions.Contains(extension))
            mediaType = Store.Domain.Enums.StoryMediaType.Video;
        else
            return ErrorModel.Create("InvalidFile");

        var saveResult = await localFileService.SaveFileAsync(file, UserStoryDirectory.StoryMedia);
        if (!saveResult.IsSuccess || string.IsNullOrWhiteSpace(saveResult.Result))
            return ErrorModel.Create("UploadFailed");

        return new UploadUserStoryMediaResponse
        {
            MediaPath = UserStoryDirectory.ToPublicUrl(saveResult.Result),
            MediaType = mediaType,
        };
    }
}
