using Store.Domain.Enums;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.CompanyStories;
using Edition.Application.Features.CompanyStories.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStories;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CompanyStoryMapperService : ICompanyStoryMapperService
{
    public PagedResult<GetAllCompanyStoryResponse> Map(PagedResult<GetAllCompanyStoryDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCompanyStoryResponse
            {
                Id = x.Id,
                Caption = x.Caption,
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                IsActive = x.IsActive,
                LikeCount = x.LikeCount,
                CommentCount = x.CommentCount,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc,
                ExpiresAtUtc = x.ExpiresAtUtc,
                CreatedByUserId = x.CreatedByUserId,
                CreatedByUserFirstName = x.CreatedByUserFirstName,
                CreatedByUserLastName = x.CreatedByUserLastName,
                Items = MapItems(x.Items)
            })
            .ToList();

        return PagedResult<GetAllCompanyStoryResponse>.Create(items, model);
    }

    public PagedResult<SearchCompanyStoryResponse> Map(PagedResult<SearchCompanyStoryDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchCompanyStoryResponse
            {
                Id = x.Id,
                Caption = x.Caption,
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                LikeCount = x.LikeCount,
                CommentCount = x.CommentCount,
                CreatedOnUtc = x.CreatedOnUtc,
                ExpiresAtUtc = x.ExpiresAtUtc,
                CreatedByUserId = x.CreatedByUserId,
                CreatedByUserFirstName = x.CreatedByUserFirstName,
                CreatedByUserLastName = x.CreatedByUserLastName,
                Items = MapItems(x.Items)
            })
            .ToList();

        return PagedResult<SearchCompanyStoryResponse>.Create(items, model);
    }

    public GetCompanyStoryResponse Map(CompanyStory model)
    {
        return new GetCompanyStoryResponse
        {
            Id = model.Id,
            Caption = model.Caption,
            CompanyId = model.CompanyId,
            IsActive = model.IsActive,
            CreatedOnUtc = model.CreatedOnUtc,
            UpdatedOnUtc = model.UpdatedOnUtc,
            ExpiresAtUtc = model.ExpiresAtUtc,
            CreatedByUserId = model.CreatedByUserId,
            Items = model.Items
                .OrderBy(x => x.Priority)
                .Select(x => new CompanyStoryItemResponse
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    Priority = x.Priority,
                    MediaType = x.MediaType,
                    DurationSeconds = x.DurationSeconds
                })
                .ToList()
        };
    }

    public GetAllCompanyStoryRequestDto Map(GetAllCompanyStoryRequest model)
    {
        return new GetAllCompanyStoryRequestDto
        {
            Caption = model.Caption,
            CompanyId = model.CompanyId,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
            CreatedByUserId = model.CreatedByUserId
        }.WithContentPolicy<CompanyStory, GetAllCompanyStoryRequestDto>(model);
    }

    public SearchCompanyStoryRequestDto Map(SearchCompanyStoryRequest model)
    {
        return new SearchCompanyStoryRequestDto
        {
            Caption = model.Caption,
            CompanyId = model.CompanyId,
            Pagination = model.Pagination
        }.WithContentPolicy<CompanyStory, SearchCompanyStoryRequestDto>(model);
    }

    private static List<CompanyStoryItemResponse> MapItems(List<CompanyStoryItemDto> items)
        => items
            .Select(x => new CompanyStoryItemResponse
            {
                Id = x.Id,
                FileName = x.FileName,
                Priority = x.Priority,
                MediaType = x.MediaType,
                DurationSeconds = x.DurationSeconds
            })
            .ToList();
}
