using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.UserStoryComments;

public record SearchUserStoryCommentRequestDto : IContentPolicyQueryDto<UserStoryComment>
{
    [QueryFilter(MemberPath = "userStoryComment.UserStoryId")]
    public int UserStoryId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<UserStoryComment, bool>>? ContentFilter { get; set; }
}

public record SearchUserStoryCommentResponseDto
{
    public int Id { get; init; }
    public string Content { get; init; } = default!;
    public int UserStoryId { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
    public int UserId { get; init; }
    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
}
