using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Queries;

public class GetPropertyHandler
    (IPropertyRepository propertyRepository, IPropertyMapperService mapper)
    : IRequestHandler<GetPropertyRequest, OperationResult<GetPropertyResponse?>>
{
    public async Task<OperationResult<GetPropertyResponse?>> Handle(GetPropertyRequest request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetAsNoTrackingAsync(request.Id);
        if (property is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(property);
        return result;
    }
}