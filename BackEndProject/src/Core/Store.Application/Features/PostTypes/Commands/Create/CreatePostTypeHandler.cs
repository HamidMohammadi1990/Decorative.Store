using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.PostTypes.Commands;

public class CreatePostTypeHandler
    (IUnitOfWork uow, IPostTypeRepository postTypeRepository)
    : IRequestHandler<CreatePostTypeRequest, OperationResult<CreatePostTypeResponse>>
{
    public async Task<OperationResult<CreatePostTypeResponse>> Handle(CreatePostTypeRequest request, CancellationToken cancellationToken)
    {
        var postType = PostType.Create(request.Title, request.Priority, request.Description);
        postTypeRepository.Add(postType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePostTypeResponse>();

        return new CreatePostTypeResponse { Id = postType.Id };
    }
}