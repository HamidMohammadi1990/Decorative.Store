using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionItems.Commands;

public class CreateSectionItemHandler
    (IUnitOfWork uow, ISectionItemRepository repository)
    : IRequestHandler<CreateSectionItemRequest, OperationResult<CreateSectionItemResponse>>
{
    public async Task<OperationResult<CreateSectionItemResponse>> Handle(CreateSectionItemRequest request, CancellationToken cancellationToken)
    {
        var model = SectionItem.Create(
            request.SectionId,
            request.Title,
            request.Priority,
            request.Icon,
            request.ImageUrl,
            request.Url,
            request.Description,
            request.IsActive);

        repository.Add(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateSectionItemResponse>();

        return new CreateSectionItemResponse { Id = model.Id };
    }
}
