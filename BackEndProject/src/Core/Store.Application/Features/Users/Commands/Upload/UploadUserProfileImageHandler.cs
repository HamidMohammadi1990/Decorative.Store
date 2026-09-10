using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record UploadUserProfileImageCommand(Microsoft.AspNetCore.Http.IFormFile Image)
    : IRequest<OperationResult<UploadUserProfileImageResponse>>;

public record UploadUserProfileImageResponse
{
    public string ImageFileName { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}

public class UploadUserProfileImageHandler(ILocalFileService localFileService)
    : IRequestHandler<UploadUserProfileImageCommand, OperationResult<UploadUserProfileImageResponse>>
{
    public async Task<OperationResult<UploadUserProfileImageResponse>> Handle(
        UploadUserProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Image is null || request.Image.Length == 0)
            return ErrorModel.Create("InvalidRequest");

        var filename = await localFileService.SaveFileAsync(request.Image, UserDirectory.UserImage);
        if (!filename.IsSuccess)
            return filename.ToGenericFailure<UploadUserProfileImageResponse>();

        var imageFileName = filename.Result!;
        return new UploadUserProfileImageResponse
        {
            ImageFileName = imageFileName,
            ImageUrl = UserDirectory.GetImageUrl(imageFileName),
        };
    }
}
