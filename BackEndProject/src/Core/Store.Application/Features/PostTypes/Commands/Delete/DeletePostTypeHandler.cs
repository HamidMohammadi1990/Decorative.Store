using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PostTypes.Commands;

public class DeletePostTypeHandler
    (IUnitOfWork uow, IPostTypeRepository postTypeRepository)
    : IRequestHandler<DeletePostTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeletePostTypeRequest request, CancellationToken cancellationToken)
    {
        var postType = await postTypeRepository.FindAsync(request.Id);
        if (postType is null)
            return ErrorModel.Create("InvalidId");

        postTypeRepository.Remove(postType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}