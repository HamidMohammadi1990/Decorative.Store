using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Commands;

public class UpdatePageHandler
    (IPageRepository pageRepository, IUnitOfWork uow)
    : IRequestHandler<UpdatePageRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePageRequest request, CancellationToken cancellationToken)
    {
        var model = await pageRepository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        model.Update(
            request.Slug,
            request.Title,
            request.Type,
            request.IsActive,
            request.MetaTitle,
            request.MetaDescription);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
