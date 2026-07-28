using Edition.Application.Features.Tags.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Tags;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface ITagMapperService : IMapper
{
    GetTagResponse Map(Tag model);
    PagedResult<GetAllTagResponse> Map(PagedResult<Tag> tags);
    GetAllTagRequestDto Map(GetAllTagRequest model);
}