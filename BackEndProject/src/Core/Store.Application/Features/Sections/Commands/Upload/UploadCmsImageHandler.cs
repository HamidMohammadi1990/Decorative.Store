using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Store.Common.Models;

namespace Edition.Application.Features.Sections.Commands;

public record UploadCmsImageResponse
{
    public string ImageFileName { get; init; } = default!;
    public string ImageUrl { get; init; } = default!;
}

public record UploadCmsImageCommand(Microsoft.AspNetCore.Http.IFormFile Image)
    : IRequest<OperationResult<UploadCmsImageResponse>>;

public class UploadCmsImageHandler
    (ILocalFileService localFileService)
    : IRequestHandler<UploadCmsImageCommand, OperationResult<UploadCmsImageResponse>>
{
    public async Task<OperationResult<UploadCmsImageResponse>> Handle(
        UploadCmsImageCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Image is null || request.Image.Length == 0)
            return ErrorModel.Create("InvalidRequest");

        var filename = await localFileService.SaveFileAsync(request.Image, CmsDirectory.CmsImage);
        if (!filename.IsSuccess)
            return filename.ToGenericFailure<UploadCmsImageResponse>();

        var imageFileName = filename.Result!;
        return new UploadCmsImageResponse
        {
            ImageFileName = imageFileName,
            ImageUrl = CmsDirectory.GetImageUrl(imageFileName),
        };
    }
}
