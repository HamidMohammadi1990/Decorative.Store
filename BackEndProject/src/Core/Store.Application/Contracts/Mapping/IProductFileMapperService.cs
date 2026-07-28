using Edition.Application.Features.ProductFiles.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductFiles;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductFileMapperService : IMapper
{
    GetProductFileResponse Map(ProductFile model);
    GetAllProductFileRequestDto Map(GetAllProductFileRequest model);
    SearchProductFileRequestDto Map(SearchProductFileRequest model);
    PagedResult<GetAllProductFileResponse> Map(PagedResult<GetAllProductFileResponseDto> model);
    PagedResult<SearchProductFileResponse> Map(PagedResult<SearchProductFileResponseDto> model);
}