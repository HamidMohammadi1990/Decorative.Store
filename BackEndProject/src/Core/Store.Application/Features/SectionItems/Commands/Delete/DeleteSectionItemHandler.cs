using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionItems.Commands;

public class DeleteSectionItemHandler
    (ISectionItemRepository repository, IUnitOfWork uow)
    : IRequestHandler<DeleteSectionItemRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteSectionItemRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        repository.Remove(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
