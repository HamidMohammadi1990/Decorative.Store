using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.CommentTopics.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CommentTopics;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CommentTopicMapperService : ICommentTopicMapperService
{
    public GetCommentTopicResponse Map(CommentTopic model)
    {
        return new GetCommentTopicResponse
        {
            Id = model.Id,
            Title = model.Title,
            Priority = model.Priority
        };
    }

    public PagedResult<GetAllCommentTopicResponse> Map(PagedResult<GetAllCommentTopicResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCommentTopicResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive,
                Priority = x.Priority
            })
            .ToList();

        return PagedResult<GetAllCommentTopicResponse>.Create(items, model);
    }

    public PagedResult<SearchCommentTopicResponse> Map(PagedResult<SearchCommentTopicResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchCommentTopicResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive,
                Priority = x.Priority
            })
            .ToList();

        return PagedResult<SearchCommentTopicResponse>.Create(items, model);
    }

    public GetAllCommentTopicRequestDto Map(GetAllCommentTopicRequest model)
    {
        return new GetAllCommentTopicRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<CommentTopic, GetAllCommentTopicRequestDto>(model);
    }

    public SearchCommentTopicRequestDto Map(SearchCommentTopicRequest model)
    {
        return new SearchCommentTopicRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<CommentTopic, SearchCommentTopicRequestDto>(model);
    }
}