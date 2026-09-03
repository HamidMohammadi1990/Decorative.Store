using Edition.Application.Common.Directories;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.RoomTypes.Queries;

public class GetAllRoomTypeHandler
    (IRoomTypeRepository repository, IRoomTypeMapperService mapper)
    : IRequestHandler<GetAllRoomTypeRequest, OperationResult<PagedResult<GetAllRoomTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllRoomTypeResponse>>> Handle(
        GetAllRoomTypeRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var result = await repository.GetAllAsync(requestModel, cancellationToken);
        return mapper.Map(result);
    }
}

public class GetRoomTypeHandler
    (IRoomTypeRepository repository)
    : IRequestHandler<GetRoomTypeRequest, OperationResult<GetRoomTypeResponse?>>
{
    public async Task<OperationResult<GetRoomTypeResponse?>> Handle(
        GetRoomTypeRequest request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        var translation = entity.Translations.FirstOrDefault(x => x.LanguageId == request.LanguageId)
            ?? entity.Translations.FirstOrDefault();

        return new GetRoomTypeResponse
        {
            Id = entity.Id,
            Code = entity.Code,
            ImageFileName = entity.ImageFileName,
            ImageUrl = ResolveImageUrl(entity.ImageFileName),
            Priority = entity.Priority,
            IsActive = entity.IsActive,
            Title = translation?.Title ?? string.Empty,
            LanguageId = translation?.LanguageId ?? request.LanguageId,
        };
    }

    private static string ResolveImageUrl(string fileName) => RoomTypeDirectory.GetImageUrl(fileName);
}

public class ListRoomTypesHandler
    (IRoomTypeRepository repository)
    : IRequestHandler<ListRoomTypesRequest, OperationResult<List<ListRoomTypeItemResponse>>>
{
    public async Task<OperationResult<List<ListRoomTypeItemResponse>>> Handle(
        ListRoomTypesRequest request,
        CancellationToken cancellationToken)
    {
        if (request.LanguageId <= 0)
            return ErrorModel.Create("InvalidRequest");

        var items = await repository.GetActiveForLanguageAsync(request.LanguageId, cancellationToken);
        var response = items
            .Select(x => new ListRoomTypeItemResponse
            {
                Id = x.Id,
                Code = x.Code,
                Title = x.Title,
                ImageFileName = x.ImageFileName,
                ImageUrl = ResolveImageUrl(x.ImageFileName),
                Priority = x.Priority,
            })
            .ToList();

        return response;
    }

    private static string ResolveImageUrl(string fileName) => RoomTypeDirectory.GetImageUrl(fileName);
}
