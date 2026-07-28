using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Tags.Queries;

public class GetTagHandler
    (ITagRepository tagRepository, ITagMapperService mapper)
    : IRequestHandler<GetTagRequest, OperationResult<GetTagResponse?>>
{
    public async Task<OperationResult<GetTagResponse?>> Handle(GetTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetAsNoTrackingAsync(request.Id);
        if (tag is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(tag);
        return result;
    }
}