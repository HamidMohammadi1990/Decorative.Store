using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Commands;

public record UpdateCurrentUserProfileImageCommand(Microsoft.AspNetCore.Http.IFormFile Image)
    : IRequest<OperationResult<UploadUserProfileImageResponse>>;

public class UpdateCurrentUserProfileImageHandler
    (ILocalFileService localFileService,
     IUserRepository userRepository,
     ICurrentUserContext currentUser,
     IUnitOfWork uow)
    : IRequestHandler<UpdateCurrentUserProfileImageCommand, OperationResult<UploadUserProfileImageResponse>>
{
    public async Task<OperationResult<UploadUserProfileImageResponse>> Handle(
        UpdateCurrentUserProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId <= 0)
            return ErrorModel.Create("AccessDenied");

        if (request.Image is null || request.Image.Length == 0)
            return ErrorModel.Create("InvalidRequest");

        var filename = await localFileService.SaveFileAsync(request.Image, UserDirectory.UserImage);
        if (!filename.IsSuccess)
            return filename.ToGenericFailure<UploadUserProfileImageResponse>();

        var user = await userRepository.FindAsync(userId, cancellationToken);
        if (user is null)
            return ErrorModel.Create("UserNotFound");

        var imageFileName = filename.Result!;
        user.SetProfileImageFileName(imageFileName);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<UploadUserProfileImageResponse>();

        return new UploadUserProfileImageResponse
        {
            ImageFileName = imageFileName,
            ImageUrl = UserDirectory.GetImageUrl(imageFileName),
        };
    }
}
