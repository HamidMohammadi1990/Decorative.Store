using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PostTypes.Queries;

public class GetPostTypeHandler 
    (IPostTypeRepository postTypeRepository, IPostTypeMapperService mapper)
    : IRequestHandler<GetPostTypeRequest, OperationResult<GetPostTypeResponse?>>
{
    public async Task<OperationResult<GetPostTypeResponse?>> Handle(GetPostTypeRequest request, CancellationToken cancellationToken)
    {
        var postType = await postTypeRepository.GetAsNoTrackingAsync(request.Id);
        if (postType is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(postType);
        return result;
    }
}