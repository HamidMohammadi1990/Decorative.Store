using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public record UpdateCompanyStoryCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyStoryCommentEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CompanyStoryCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    public string Content { get; init; } = default!;
}
