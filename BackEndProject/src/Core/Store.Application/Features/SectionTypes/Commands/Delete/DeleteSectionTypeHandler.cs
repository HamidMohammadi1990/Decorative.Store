using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Commands;

public class DeleteSectionTypeHandler
    (ISectionTypeRepository repository, ISectionRepository sectionRepository, IUnitOfWork uow)
    : IRequestHandler<DeleteSectionTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteSectionTypeRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        if (await sectionRepository.AnyAsync(x => x.SectionTypeId == request.Id))
            return ErrorModel.Create("HasSections");

        repository.Remove(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
