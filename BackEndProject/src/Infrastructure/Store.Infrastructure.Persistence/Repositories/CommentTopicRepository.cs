using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CommentTopics;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CommentTopicRepository
     (EditionDbContext context)
    : Repository<CommentTopic>(context), ICommentTopicRepository
{
    public async Task<PagedResult<GetAllCommentTopicResponseDto>> GetAllAsync(GetAllCommentTopicRequestDto request)
    {
        var topics = await Context.CommentTopic            
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .Select(x => new GetAllCommentTopicResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                IsActive = x.IsActive
            })
            .ToPagedAsync(request.Pagination);
        
        return topics;
    }

    public async Task<PagedResult<SearchCommentTopicResponseDto>> SearchAsync(SearchCommentTopicRequestDto request)
    {
        var topics = await Context.CommentTopic
            .ApplyQueryFilters(request)
            .AsNoTracking()
            .Select(x => new SearchCommentTopicResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                IsActive = x.IsActive
            })
            .ToPagedAsync(request.Pagination);

        return topics;
    }
}