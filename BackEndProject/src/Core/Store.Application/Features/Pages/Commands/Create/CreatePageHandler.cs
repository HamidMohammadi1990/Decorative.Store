using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Pages.Commands;

public class CreatePageHandler
    (IUnitOfWork uow, IPageRepository pageRepository)
    : IRequestHandler<CreatePageRequest, OperationResult<CreatePageResponse>>
{
    public async Task<OperationResult<CreatePageResponse>> Handle(CreatePageRequest request, CancellationToken cancellationToken)
    {
        var model = Page.Create(request.Type, request.IsActive);
        model.UpsertTranslation(
            request.LanguageId,
            request.Title,
            request.Slug,
            request.MetaTitle,
            request.MetaDescription);

        pageRepository.Add(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePageResponse>();

        return new CreatePageResponse { Id = model.Id };
    }
}
