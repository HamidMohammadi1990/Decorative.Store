using Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProductOrderItemAttachmentTypes;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IProductOrderItemAttachmentTypeMapperService : IMapper
{
    GetProductOrderItemAttachmentTypeResponse Map(ProductOrderItemAttachmentType model);
    GetAllProductOrderItemAttachmentTypeRequestDto Map(GetAllProductOrderItemAttachmentTypeRequest model);
    SearchProductOrderItemAttachmentTypeRequestDto Map(SearchProductOrderItemAttachmentTypeRequest model);
    PagedResult<GetAllProductOrderItemAttachmentTypeResponse> Map(PagedResult<ProductOrderItemAttachmentType> model);
    PagedResult<SearchProductOrderItemAttachmentTypeResponse> MapToSearch(PagedResult<ProductOrderItemAttachmentType> model);
}
