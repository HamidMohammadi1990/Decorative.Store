using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Tags.Queries;

public class GetAllTagHandler
    (ITagRepository tagRepository, ITagMapperService mapper)
    : IRequestHandler<GetAllTagRequest, OperationResult<PagedResult<GetAllTagResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllTagResponse>>> Handle(GetAllTagRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var tags = await tagRepository.GetAllAsync(requestModel);
        return mapper.Map(tags);
    }
}