using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Commands;

public class DeletePageHandler
    (IPageRepository pageRepository, IPageSectionRepository pageSectionRepository, IUnitOfWork uow)
    : IRequestHandler<DeletePageRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeletePageRequest request, CancellationToken cancellationToken)
    {
        var model = await pageRepository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        if (await pageSectionRepository.AnyAsync(x => x.PageId == request.Id))
            return ErrorModel.Create("HasPageSections");

        pageRepository.Remove(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
