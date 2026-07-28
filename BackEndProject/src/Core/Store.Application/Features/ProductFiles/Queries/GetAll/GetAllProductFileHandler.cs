using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductFiles.Queries;

public class GetAllProductFileHandler
    (IProductFileRepository productFileRepository, IProductFileMapperService mapper)
    : IRequestHandler<GetAllProductFileRequest, OperationResult<PagedResult<GetAllProductFileResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductFileResponse>>> Handle(GetAllProductFileRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var productFiles = await productFileRepository.GetAllAsync(requestModel);
        var result = mapper.Map(productFiles);
        return result;
    }
}