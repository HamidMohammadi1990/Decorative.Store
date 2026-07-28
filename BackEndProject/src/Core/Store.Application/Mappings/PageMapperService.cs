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
    public PagedResult<GetAllPageResponse> Map(PagedResult<Page> model)
    {
        var items = model.Items.Select(x => new GetAllPageResponse
        {
            Id = x.Id,
            Slug = x.Slug,
            Title = x.Title,
            Type = x.Type,
            IsActive = x.IsActive,
            MetaTitle = x.MetaTitle,
            MetaDescription = x.MetaDescription
        }).ToList();

        return PagedResult<GetAllPageResponse>.Create(items, model);
    }

    public PagedResult<SearchPageResponse> MapToSearch(PagedResult<Page> model)
    {
        var items = model.Items.Select(x => new SearchPageResponse
        {
            Id = x.Id,
            Slug = x.Slug,
            Title = x.Title,
            Type = x.Type,
            MetaTitle = x.MetaTitle,
            MetaDescription = x.MetaDescription
        }).ToList();

        return PagedResult<SearchPageResponse>.Create(items, model);
    }

    public GetPageResponse Map(Page model)
    {
        return new GetPageResponse
        {
            Id = model.Id,
            Slug = model.Slug,
            Title = model.Title,
            Type = model.Type,
            IsActive = model.IsActive,
            MetaTitle = model.MetaTitle,
            MetaDescription = model.MetaDescription
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
            Pagination = model.Pagination
        }.WithContentPolicy<Page, GetAllPageRequestDto>(model);
    }

    public SearchPageRequestDto Map(SearchPageRequest model)
    {
        return new SearchPageRequestDto
        {
            Slug = model.Slug,
            Title = model.Title,
            Type = model.Type,
            Pagination = model.Pagination
        }.WithContentPolicy<Page, SearchPageRequestDto>(model);
    }
}
