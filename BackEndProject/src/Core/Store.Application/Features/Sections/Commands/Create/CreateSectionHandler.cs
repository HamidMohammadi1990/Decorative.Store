using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Sections.Commands;

public class CreateSectionHandler
    (IUnitOfWork uow, ISectionRepository repository)
    : IRequestHandler<CreateSectionRequest, OperationResult<CreateSectionResponse>>
{
    public async Task<OperationResult<CreateSectionResponse>> Handle(CreateSectionRequest request, CancellationToken cancellationToken)
    {
        var model = Section.Create(
            request.SectionTypeId,
            request.ParentId,
            request.Title,
            request.Description,
            request.Url,
            request.ImageUrl,
            request.StartDateOnUtc,
            request.EndDateOnUtc,
            request.IsActive);

        repository.Add(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateSectionResponse>();

        return new CreateSectionResponse { Id = model.Id };
    }
}
