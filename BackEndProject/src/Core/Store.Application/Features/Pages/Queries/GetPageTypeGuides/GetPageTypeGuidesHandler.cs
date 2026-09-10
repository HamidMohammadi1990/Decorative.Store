using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageTypeGuidesHandler(IPageTypeGuideRepository repository)
    : IRequestHandler<GetPageTypeGuidesRequest, OperationResult<List<GetPageTypeGuidesResponse>>>
{
    public async Task<OperationResult<List<GetPageTypeGuidesResponse>>> Handle(
        GetPageTypeGuidesRequest request,
        CancellationToken cancellationToken)
    {
        var guides = await repository.GetAllAsync(cancellationToken);

        var response = guides
            .Select(x => new GetPageTypeGuidesResponse
            {
                Type = x.Type,
                AdminDescription = x.AdminDescription,
            })
            .ToList();

        return response;
    }
}
