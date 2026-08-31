using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserStoryComments.Queries;

public record SearchUserStoryCommentRequest : ContentPolicyRequest<UserStoryComment>, IRequest<OperationResult<PagedResult<SearchUserStoryCommentResponse>>>
{
    [JsonConverter(typeof(UserStoryEncryptor))]
    public int UserStoryId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}

public record SearchUserStoryCommentResponse
{
    [JsonConverter(typeof(UserStoryCommentEncryptor))]
    public int Id { get; init; }

    public string Content { get; init; } = default!;

    [JsonConverter(typeof(UserStoryEncryptor))]
    public int UserStoryId { get; init; }

    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ApprovedOnUtc { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserFirstName { get; init; } = default!;
    public string UserLastName { get; init; } = default!;
}
