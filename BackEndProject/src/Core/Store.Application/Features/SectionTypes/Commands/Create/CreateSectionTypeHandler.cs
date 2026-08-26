using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Commands;

public class CreateSectionTypeHandler
    (IUnitOfWork uow, ISectionTypeRepository repository)
    : IRequestHandler<CreateSectionTypeRequest, OperationResult<CreateSectionTypeResponse>>
{
    public async Task<OperationResult<CreateSectionTypeResponse>> Handle(CreateSectionTypeRequest request, CancellationToken cancellationToken)
    {
        var model = SectionType.Create(request.IsActive);
        model.UpsertTranslation(request.LanguageId, request.Name);

        repository.Add(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateSectionTypeResponse>();

        return new CreateSectionTypeResponse { Id = model.Id };
    }
}
