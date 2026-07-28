using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PageSections.Queries;

public class GetPageSectionHandler
    (IPageSectionRepository repository, IPageSectionMapperService mapper)
    : IRequestHandler<GetPageSectionRequest, OperationResult<GetPageSectionResponse?>>
{
    public async Task<OperationResult<GetPageSectionResponse?>> Handle(GetPageSectionRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsNoTrackingAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(model);
    }
}
