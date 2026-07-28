using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Commands;

public class DeleteSectionHandler
    (ISectionRepository repository, IPageSectionRepository pageSectionRepository, ISectionItemRepository sectionItemRepository, IUnitOfWork uow)
    : IRequestHandler<DeleteSectionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteSectionRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.FindAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        if (await repository.AnyAsync(x => x.ParentId == request.Id))
            return ErrorModel.Create("HasChildren");

        if (await pageSectionRepository.AnyAsync(x => x.SectionId == request.Id))
            return ErrorModel.Create("HasPageSections");

        if (await sectionItemRepository.AnyAsync(x => x.SectionId == request.Id))
            return ErrorModel.Create("HasSectionItems");

        repository.Remove(model);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
