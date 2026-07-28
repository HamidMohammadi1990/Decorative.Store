using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Tags.Commands;

public class DeleteTagHandler
    (IUnitOfWork uow, ITagRepository tagRepository)
    : IRequestHandler<DeleteTagRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.FindAsync(request.Id);
        if (tag is null)
            return ErrorModel.Create("InvalidId");

        tagRepository.Remove(tag);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}