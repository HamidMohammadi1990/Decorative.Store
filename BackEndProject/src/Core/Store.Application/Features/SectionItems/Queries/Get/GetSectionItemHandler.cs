using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionItems.Queries;

public class GetSectionItemHandler
    (ISectionItemRepository repository, ISectionItemMapperService mapper)
    : IRequestHandler<GetSectionItemRequest, OperationResult<GetSectionItemResponse?>>
{
    public async Task<OperationResult<GetSectionItemResponse?>> Handle(GetSectionItemRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsNoTrackingAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(model);
    }
}
