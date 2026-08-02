using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Localization;
using Edition.Application.Features.Properties.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Properties;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPropertyMapperService : IMapper
{
    GetPropertyResponse Map(Property model, string title, string? description);
    GetAllPropertyRequestDto Map(GetAllPropertyRequest model);
    PagedResult<GetAllPropertyResponse> Map(PagedResult<GetAllPropertyDto> model);
}
