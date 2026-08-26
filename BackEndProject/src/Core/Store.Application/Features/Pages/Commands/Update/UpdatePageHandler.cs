using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Commands;

public class UpdatePageHandler
    (IUnitOfWork uow, IPageRepository pageRepository)
    : IRequestHandler<UpdatePageRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePageRequest request, CancellationToken cancellationToken)
    {
        var page = await pageRepository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (page is null)
            return ErrorModel.Create("InvalidId");

        page.Update(
            request.Type,
            request.IsActive,
            request.LanguageId,
            request.Title,
            request.Slug,
            request.MetaTitle,
            request.MetaDescription);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        return saveChangesResult.IsSuccess
            ? OperationResult.Success()
            : saveChangesResult;
    }
}
