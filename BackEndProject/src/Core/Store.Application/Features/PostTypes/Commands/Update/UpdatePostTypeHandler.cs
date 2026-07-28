using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PostTypes.Commands;

public class UpdatePostTypeHandler
    (IUnitOfWork uow, IPostTypeRepository postTypeRepository)
    : IRequestHandler<UpdatePostTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePostTypeRequest request, CancellationToken cancellationToken)
    {
        var postType = await postTypeRepository.FindAsync(request.Id);
        if (postType is null)
            return ErrorModel.Create("InvalidId");

        postType.Update(request.Title, request.Description, request.IsActive, request.Priority);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}