using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PageSections.Commands;

public class DeletePageSectionHandler
    (IPageSectionRepository repository, IUnitOfWork uow)
    : IRequestHandler<DeletePageSectionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeletePageSectionRequest request, CancellationToken cancellationToken)
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
