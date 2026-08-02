using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.PropertyItems.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PropertyItems;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPropertyItemMapperService : IMapper
{
    GetPropertyItemResponse Map(PropertyItem model, string title);
    GetAllPropertyItemRequestDto Map(GetAllPropertyItemRequest model);
    PagedResult<GetAllPropertyItemResponse> Map(PagedResult<GetAllPropertyItemDto> model);
}
