using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.PostTypes.Queries;
using Store.Domain.Dtos.PostTypes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class PostTypeMapperService : IPostTypeMapperService
{
    public GetAllPostTypeRequestDto Map(GetAllPostTypeRequest model)
    {
        return new GetAllPostTypeRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<PostType, GetAllPostTypeRequestDto>(model);
    }

    public SearchPostTypeRequestDto Map(SearchPostTypeRequest model)
    {
        return new SearchPostTypeRequestDto
        {
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<PostType, SearchPostTypeRequestDto>(model);
    }

    public GetPostTypeResponse Map(PostType model)
    {
        return new GetPostTypeResponse
        {
            Id = model.Id,
            Title = model.Title,
            Priority = model.Priority,
            IsActive = model.IsActive,
            Description = model.Description,
        };
    }

    public PagedResult<GetAllPostTypeResponse> Map(PagedResult<GetAllPostTypeResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllPostTypeResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive,
                Priority = x.Priority,
                Description = x.Description
            })
            .ToList();

        return PagedResult<GetAllPostTypeResponse>.Create(items, model);
    }

    public PagedResult<SearchPostTypeResponse> Map(PagedResult<SearchPostTypeResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchPostTypeResponse
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                Description = x.Description
            })
            .ToList();

        return PagedResult<SearchPostTypeResponse>.Create(items, model);
    }
}
