using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.CompanyComments.Queries;

public record GetCompanyCommentResponse
{
    [JsonConverter(typeof(CompanyCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public DateTime CreatedOnUtc { get; init; } = DateTime.UtcNow;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int Rate { get; init; }
    public CommentStatusType StatusType { get; init; }
}