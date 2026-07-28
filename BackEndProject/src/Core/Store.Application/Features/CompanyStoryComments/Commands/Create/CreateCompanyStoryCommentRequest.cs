using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public record CreateCompanyStoryCommentRequest : IRequest<OperationResult<CreateCompanyStoryCommentResponse>>
{
    [JsonConverter(typeof(CompanyStoryCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }

    public string Content { get; init; } = default!;
}
