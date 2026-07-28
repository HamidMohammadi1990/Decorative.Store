using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Tags.Commands;

public class UpdateTagHandler
    (IUnitOfWork uow, ITagRepository tagRepository)
    : IRequestHandler<UpdateTagRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.FindAsync(request.Id);
        if (tag is null)
            return ErrorModel.Create("InvalidId");

        tag.Update(request.Title, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}