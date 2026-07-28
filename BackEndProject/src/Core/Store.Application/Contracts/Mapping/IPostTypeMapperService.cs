using Edition.Application.Features.PostTypes.Queries;
using Store.Domain.Dtos.PostTypes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPostTypeMapperService : IMapper
{
    GetPostTypeResponse Map(PostType model);
    GetAllPostTypeRequestDto Map(GetAllPostTypeRequest model);
    SearchPostTypeRequestDto Map(SearchPostTypeRequest model);
    PagedResult<GetAllPostTypeResponse> Map(PagedResult<GetAllPostTypeResponseDto> model);
    PagedResult<SearchPostTypeResponse> Map(PagedResult<SearchPostTypeResponseDto> model);
}