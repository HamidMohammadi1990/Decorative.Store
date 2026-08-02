using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Products.Queries;
using Store.Domain.Dtos.Products;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductMapperService : IMapper
{
    GetProductResponse Map(Product model, string title, string slug, string description);
    PagedResult<GetAllProductResponse> Map(PagedResult<GetAllProductResponseDto> model);
    GetAllProductRequestDto Map(GetAllProductRequest model);
}
