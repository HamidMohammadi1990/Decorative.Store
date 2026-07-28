using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Queries;

public class GetSectionTypeHandler
    (ISectionTypeRepository repository, ISectionTypeMapperService mapper)
    : IRequestHandler<GetSectionTypeRequest, OperationResult<GetSectionTypeResponse?>>
{
    public async Task<OperationResult<GetSectionTypeResponse?>> Handle(GetSectionTypeRequest request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsNoTrackingAsync(request.Id);
        if (model is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(model);
    }
}
