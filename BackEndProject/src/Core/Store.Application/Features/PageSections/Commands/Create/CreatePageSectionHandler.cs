using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.PageSections.Commands;

public class CreatePageSectionHandler
    (IUnitOfWork uow, IPageSectionRepository repository)
    : IRequestHandler<CreatePageSectionRequest, OperationResult<CreatePageSectionResponse>>
{
    public async Task<OperationResult<CreatePageSectionResponse>> Handle(CreatePageSectionRequest request, CancellationToken cancellationToken)
    {
        var model = PageSection.Create(request.PageId, request.SectionId, request.Priority, request.AdminDescription);

        repository.Add(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePageSectionResponse>();

        return new CreatePageSectionResponse { Id = model.Id };
    }
}
