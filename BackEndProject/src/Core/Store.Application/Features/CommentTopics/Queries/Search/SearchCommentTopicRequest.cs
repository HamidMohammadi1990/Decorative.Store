using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CommentTopics.Queries;

public record SearchCommentTopicRequest : ContentPolicyRequest<CommentTopic>, IRequest<OperationResult<PagedResult<SearchCommentTopicResponse>>>
{
    public string? Title { get; init; }
    public bool? IsActive { get; init; } = true;
    public PagedRequest Pagination { get; init; } = default!;
}