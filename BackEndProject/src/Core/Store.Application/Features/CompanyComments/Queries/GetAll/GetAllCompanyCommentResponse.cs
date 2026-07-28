using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Enums;

namespace Edition.Application.Features.CompanyComments.Queries;

public record GetAllCompanyCommentResponse
{
    [JsonConverter(typeof(CompanyCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserName { get; init; } = default!;
    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public string CompanyName { get; init; } = default!;
    public DateTime CreatedOnUtc { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int Rate { get; init; }
    public CommentStatusType StatusType { get; init; }
}