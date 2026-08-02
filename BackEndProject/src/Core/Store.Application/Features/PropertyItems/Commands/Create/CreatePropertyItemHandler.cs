using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.PropertyItems.Commands;

public class CreatePropertyItemHandler
    (IUnitOfWork uow, IPropertyItemRepository propertyItemrepository)
    : IRequestHandler<CreatePropertyItemRequest, OperationResult<CreatePropertyItemResponse>>
{
    public async Task<OperationResult<CreatePropertyItemResponse>> Handle(CreatePropertyItemRequest request, CancellationToken cancellationToken)
    {
        var propertyItem = PropertyItem.Create(request.Code, request.PropertyId, request.Priority);
        propertyItem.UpsertTranslation(request.LanguageId, request.Title);
        propertyItemrepository.Add(propertyItem);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePropertyItemResponse>();

        return new CreatePropertyItemResponse { Id = propertyItem.Id };
    }
}
