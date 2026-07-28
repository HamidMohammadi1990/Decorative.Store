using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Queries;

public class GetSectionHandler
    (ISectionRepository repository, ISectionMapperService mapper)
    : IRequestHandler<GetSectionRequest, OperationResult<GetSectionResponse?>>
{
    public async Task<OperationResult<GetSectionResponse?>> Handle(GetSectionRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsNoTrackingAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(model);
    }
}
