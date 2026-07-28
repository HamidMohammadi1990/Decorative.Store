using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.SectionItems.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.SectionItems;

namespace Edition.Application.Mappings;

public class SectionItemMapperService : ISectionItemMapperService
{
    public PagedResult<GetAllSectionItemResponse> Map(PagedResult<SectionItem> model)
    {
        var items = model.Items.Select(x => new GetAllSectionItemResponse
        {
            Id = x.Id,
            SectionId = x.SectionId,
            Title = x.Title,
            Priority = x.Priority,
            Icon = x.Icon,
            ImageUrl = x.ImageUrl,
            Url = x.Url,
            Description = x.Description,
            IsActive = x.IsActive
        }).ToList();

        return PagedResult<GetAllSectionItemResponse>.Create(items, model);
    }

    public PagedResult<SearchSectionItemResponse> MapToSearch(PagedResult<SectionItem> model)
    {
        var items = model.Items.Select(x => new SearchSectionItemResponse
        {
            Id = x.Id,
            SectionId = x.SectionId,
            Title = x.Title,
            Priority = x.Priority,
            Icon = x.Icon,
            ImageUrl = x.ImageUrl,
            Url = x.Url,
            IsActive = x.IsActive
        }).ToList();

        return PagedResult<SearchSectionItemResponse>.Create(items, model);
    }

    public GetSectionItemResponse Map(SectionItem model)
    {
        return new GetSectionItemResponse
        {
            Id = model.Id,
            SectionId = model.SectionId,
            Title = model.Title,
            Priority = model.Priority,
            Icon = model.Icon,
            ImageUrl = model.ImageUrl,
            Url = model.Url,
            Description = model.Description,
            IsActive = model.IsActive
        };
    }

    public GetAllSectionItemRequestDto Map(GetAllSectionItemRequest model)
    {
        return new GetAllSectionItemRequestDto
        {
            SectionId = model.SectionId,
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<SectionItem, GetAllSectionItemRequestDto>(model);
    }

    public SearchSectionItemRequestDto Map(SearchSectionItemRequest model)
    {
        return new SearchSectionItemRequestDto
        {
            SectionId = model.SectionId,
            Title = model.Title,
            Pagination = model.Pagination
        }.WithContentPolicy<SectionItem, SearchSectionItemRequestDto>(model);
    }
}
