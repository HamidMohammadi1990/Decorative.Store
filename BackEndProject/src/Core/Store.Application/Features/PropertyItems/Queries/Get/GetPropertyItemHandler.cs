using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Queries;

public class GetPropertyItemHandler
    (IPropertyItemRepository propertyItemRepository, IPropertyItemMapperService mapper)
    : IRequestHandler<GetPropertyItemRequest, OperationResult<GetPropertyItemResponse?>>
{
    public async Task<OperationResult<GetPropertyItemResponse?>> Handle(GetPropertyItemRequest request, CancellationToken cancellationToken)
    {
        var tag = await propertyItemRepository.GetAsNoTrackingAsync(request.Id);
        if (tag is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(tag);
        return result;
    }
}