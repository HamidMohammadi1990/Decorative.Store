using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionItems.Commands;

public class UpdateSectionItemHandler
    (ISectionItemRepository repository, IUnitOfWork uow)
    : IRequestHandler<UpdateSectionItemRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateSectionItemRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        model.Update(
            request.SectionId,
            request.Priority,
            request.Icon,
            request.ImageUrl,
            request.IsActive,
            request.LanguageId,
            request.Title,
            request.Description,
            request.Url);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
