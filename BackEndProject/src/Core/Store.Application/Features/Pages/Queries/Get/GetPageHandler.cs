using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageHandler
    (IPageRepository pageRepository, IPageMapperService mapper)
    : IRequestHandler<GetPageRequest, OperationResult<GetPageResponse?>>
{
    public async Task<OperationResult<GetPageResponse?>> Handle(GetPageRequest request, CancellationToken cancellationToken)
    {
        var page = await pageRepository.GetAsNoTrackingAsync(request.Id);
        if (page is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(page);
    }
}
