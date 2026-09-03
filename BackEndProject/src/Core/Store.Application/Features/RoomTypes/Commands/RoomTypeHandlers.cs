using Edition.Application.Common.Directories;
using Edition.Application.Common.Utilities.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.RoomTypes.Commands;

public class CreateRoomTypeHandler
    (IUnitOfWork uow, IRoomTypeRepository repository)
    : IRequestHandler<CreateRoomTypeRequest, OperationResult<CreateRoomTypeResponse>>
{
    public async Task<OperationResult<CreateRoomTypeResponse>> Handle(
        CreateRoomTypeRequest request,
        CancellationToken cancellationToken)
    {
        var entity = RoomType.Create(
            request.Code.Trim(),
            request.ImageFileName.Trim(),
            request.Priority);
        entity.UpsertTranslation(request.LanguageId, request.Title.Trim());
        repository.Add(entity);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateRoomTypeResponse>();

        return new CreateRoomTypeResponse { Id = entity.Id };
    }
}

public class UpdateRoomTypeHandler
    (IUnitOfWork uow, IRoomTypeRepository repository)
    : IRequestHandler<UpdateRoomTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var entity = await repository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        entity.Update(
            request.Code.Trim(),
            request.ImageFileName.Trim(),
            request.Priority,
            request.IsActive,
            request.LanguageId,
            request.Title.Trim());

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}

public class DeleteRoomTypeHandler
    (IUnitOfWork uow, IRoomTypeRepository repository)
    : IRequestHandler<DeleteRoomTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var entity = await repository.FindAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        repository.Remove(entity);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}

public class UploadRoomTypeImageHandler
    (ILocalFileService localFileService)
    : IRequestHandler<UploadRoomTypeImageCommand, OperationResult<UploadRoomTypeImageResponse>>
{
    public async Task<OperationResult<UploadRoomTypeImageResponse>> Handle(
        UploadRoomTypeImageCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Image is null || request.Image.Length == 0)
            return ErrorModel.Create("InvalidRequest");

        var filename = await localFileService.SaveFileAsync(request.Image, RoomTypeDirectory.RoomTypeImage);
        if (!filename.IsSuccess)
            return filename.ToGenericFailure<UploadRoomTypeImageResponse>();

        var imageFileName = filename.Result!;
        return new UploadRoomTypeImageResponse
        {
            ImageFileName = imageFileName,
            ImageUrl = RoomTypeDirectory.GetImageUrl(imageFileName),
        };
    }
}

public record UploadRoomTypeImageCommand(Microsoft.AspNetCore.Http.IFormFile Image)
    : IRequest<OperationResult<UploadRoomTypeImageResponse>>;
