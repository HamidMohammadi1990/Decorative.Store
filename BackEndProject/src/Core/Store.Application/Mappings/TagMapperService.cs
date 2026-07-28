using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Tags.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Tags;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class TagMapperService : ITagMapperService
{
    public GetAllTagRequestDto Map(GetAllTagRequest model)
    {
        return new GetAllTagRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<Tag, GetAllTagRequestDto>(model);
    }

    public GetTagResponse Map(Tag model)
    {
        return new GetTagResponse
        {
            Id = model.Id,
            Title = model.Title,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllTagResponse> Map(PagedResult<Tag> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllTagResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<GetAllTagResponse>.Create(items, model);
    }
}