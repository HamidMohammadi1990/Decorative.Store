using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Commands;

public class UpdateSectionHandler
    (ISectionRepository repository, IUnitOfWork uow)
    : IRequestHandler<UpdateSectionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateSectionRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        model.Update(
            request.SectionTypeId,
            request.ParentId,
            request.ImageUrl,
            request.StartDateOnUtc,
            request.EndDateOnUtc,
            request.IsActive,
            request.LanguageId,
            request.Title,
            request.Url,
            request.Description);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
