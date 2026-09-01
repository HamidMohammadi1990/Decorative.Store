using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.SectionItems.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionItems;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class SectionItemMapperService : ISectionItemMapperService
{
    public PagedResult<GetAllSectionItemResponse> Map(PagedResult<GetAllSectionItemResponseDto> model)
    {
        var items = model.Items.Select(x => new GetAllSectionItemResponse
        {
            Id = x.Id,
            SectionId = x.SectionId,
            Priority = x.Priority,
            Icon = x.Icon,
            ImageUrl = x.ImageUrl,
            IsActive = x.IsActive,
            SectionTitle = x.SectionTitle,
            SectionTypeName = x.SectionTypeName,
            Translations = x.Translations,
        }).ToList();

        return PagedResult<GetAllSectionItemResponse>.Create(items, model);
    }

    public PagedResult<SearchSectionItemResponse> MapToSearch(PagedResult<SearchSectionItemResponseDto> model)
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
            Description = x.Description,
            IsActive = x.IsActive,
        }).ToList();

        return PagedResult<SearchSectionItemResponse>.Create(items, model);
    }

    public GetSectionItemResponse Map(SectionItem model, string title, string? description, string? url)
    {
        return new GetSectionItemResponse
        {
            Id = model.Id,
            SectionId = model.SectionId,
            Title = title,
            Priority = model.Priority,
            Icon = model.Icon,
            ImageUrl = model.ImageUrl,
            Url = url,
            Description = description,
            IsActive = model.IsActive,
        };
    }

    public GetAllSectionItemRequestDto Map(GetAllSectionItemRequest model)
    {
        return new GetAllSectionItemRequestDto
        {
            SectionId = model.SectionId,
            Title = model.Title,
            Pagination = model.Pagination,
        }.WithContentPolicy<SectionItem, GetAllSectionItemRequestDto>(model);
    }

    public SearchSectionItemRequestDto Map(SearchSectionItemRequest model)
    {
        return new SearchSectionItemRequestDto
        {
            SectionId = model.SectionId,
            Title = model.Title,
            Pagination = model.Pagination,
        }.WithContentPolicy<SectionItem, SearchSectionItemRequestDto>(model);
    }
}
