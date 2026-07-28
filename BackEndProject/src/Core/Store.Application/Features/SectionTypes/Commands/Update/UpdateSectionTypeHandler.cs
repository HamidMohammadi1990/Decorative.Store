using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Commands;

public class UpdateSectionTypeHandler
    (ISectionTypeRepository repository, IUnitOfWork uow)
    : IRequestHandler<UpdateSectionTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateSectionTypeRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        model.Update(request.Name, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
