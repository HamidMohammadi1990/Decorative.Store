using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PageSections.Commands;

public class UpdatePageSectionHandler
    (IPageSectionRepository repository, IUnitOfWork uow)
    : IRequestHandler<UpdatePageSectionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePageSectionRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        model.Update(request.PageId, request.SectionId, request.Priority, request.AdminDescription);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
