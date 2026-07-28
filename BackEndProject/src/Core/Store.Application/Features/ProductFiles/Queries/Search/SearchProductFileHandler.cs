using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFiles.Queries;

public class SearchProductFileHandler
    (IProductFileRepository productFileRepository, IProductFileMapperService mapper)
    : IRequestHandler<SearchProductFileRequest, OperationResult<PagedResult<SearchProductFileResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductFileResponse>>> Handle(SearchProductFileRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productFiles = await productFileRepository.SearchAsync(requestModel);
        var result = mapper.Map(productFiles);
        return result;
    }
}