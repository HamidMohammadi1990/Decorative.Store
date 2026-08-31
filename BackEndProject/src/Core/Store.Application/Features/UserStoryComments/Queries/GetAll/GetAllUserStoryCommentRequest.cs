using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserStoryComments.Queries;

public record GetAllUserStoryCommentRequest : ContentPolicyRequest<UserStoryComment>, IRequest<OperationResult<PagedResult<GetAllUserStoryCommentResponse>>>
{
    [JsonConverter(typeof(UserStoryNullableEncryptor))]
    public int? UserStoryId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public bool? IsApproved { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllUserStoryCommentResponse
{
    [JsonConverter(typeof(UserStoryCommentEncryptor))]
    public int Id { get; init; }

    public string Content { get; init; } = default!;
    public bool IsApproved { get; init; }

    [JsonConverter(typeof(UserStoryEncryptor))]
    public int UserStoryId { get; init; }

    public string UserStoryTitle { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserFirstName { get; init; } = default!;
    public string UserLastName { get; init; } = default!;

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? ApprovedByUserId { get; init; }

    public string? ApprovedByUserFirstName { get; init; }
    public string? ApprovedByUserLastName { get; init; }
}
