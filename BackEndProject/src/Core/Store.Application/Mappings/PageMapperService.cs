using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Pages.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Pages;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class PageMapperService : IPageMapperService
{
    public PagedResult<GetAllPageResponse> Map(PagedResult<GetAllPageResponseDto> model)
    {
        var items = model.Items.Select(x => new GetAllPageResponse
        {
            Id = x.Id,
            Type = x.Type,
            IsActive = x.IsActive,
            AdminDescription = x.AdminDescription,
            Translations = x.Translations,
        }).ToList();

        return PagedResult<GetAllPageResponse>.Create(items, model);
    }

    public PagedResult<SearchPageResponse> MapToSearch(PagedResult<SearchPageResponseDto> model)
    {
        var items = model.Items.Select(x => new SearchPageResponse
        {
            Id = x.Id,
            Slug = x.Slug,
            Title = x.Title,
            Type = x.Type,
            MetaTitle = x.MetaTitle,
            MetaDescription = x.MetaDescription,
        }).ToList();

        return PagedResult<SearchPageResponse>.Create(items, model);
    }

    public GetPageResponse Map(
        Page model,
        string title,
        string slug,
        string? metaTitle,
        string? metaDescription)
    {
        return new GetPageResponse
        {
            Id = model.Id,
            Slug = slug,
            Title = title,
            Type = model.Type,
            IsActive = model.IsActive,
            MetaTitle = metaTitle,
            MetaDescription = metaDescription,
        };
    }

    public GetAllPageRequestDto Map(GetAllPageRequest model)
    {
        return new GetAllPageRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            Type = model.Type,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        }.WithContentPolicy<Page, GetAllPageRequestDto>(model);
    }

    public SearchPageRequestDto Map(SearchPageRequest model)
    {
        return new SearchPageRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            Type = model.Type,
            Pagination = model.Pagination,
        }.WithContentPolicy<Page, SearchPageRequestDto>(model);
    }
}
