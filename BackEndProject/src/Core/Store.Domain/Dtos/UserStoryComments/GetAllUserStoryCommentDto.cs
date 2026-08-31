using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.UserStoryComments;

public record GetAllUserStoryCommentRequestDto : IContentPolicyQueryDto<UserStoryComment>
{
    [QueryFilter(MemberPath = "userStory.Id")]
    public int? UserStoryId { get; init; }

    [QueryFilter(MemberPath = "userStoryComment.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "userStoryComment.ApprovedByUserId")]
    public int? ApprovedByUserId { get; init; }

    [QueryFilter(MemberPath = "userStoryComment.IsApproved")]
    public bool? IsApproved { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<UserStoryComment, bool>>? ContentFilter { get; set; }
}

public record GetAllUserStoryCommentResponseDto
{
    public int Id { get; init; }
    public string Content { get; init; } = default!;
    public bool IsApproved { get; init; }
    public int UserStoryId { get; init; }
    public string UserStoryTitle { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }
    public int UserId { get; init; }
    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public int? ApprovedByUserId { get; init; }
    public string? ApprovedByUserFirstName { get; init; }
    public string? ApprovedByUserLastName { get; init; }
}
