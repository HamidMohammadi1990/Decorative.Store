using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Tags.Commands;

public class CreateTagHandler
    (IUnitOfWork uow, ITagRepository tagRepository)
    : IRequestHandler<CreateTagRequest, OperationResult<CreateTagResponse>>
{
    public async Task<OperationResult<CreateTagResponse>> Handle(CreateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = Tag.Create(request.Title);

        tagRepository.Add(tag);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateTagResponse>();

        return new CreateTagResponse { Id = tag.Id };
    }
}