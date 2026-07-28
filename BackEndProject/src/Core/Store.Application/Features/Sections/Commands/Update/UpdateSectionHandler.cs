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
        var model = await repository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        model.Update(
            request.SectionTypeId,
            request.ParentId,
            request.Title,
            request.Description,
            request.Url,
            request.ImageUrl,
            request.StartDateOnUtc,
            request.EndDateOnUtc,
            request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
