using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyComments.Commands;

public record CreateCompanyCommentRequest : IRequest<OperationResult<CreateCompanyCommentResponse>>
{
    [JsonConverter(typeof(CompanyCommentNullableEncryptor))]
    public int? ParentId { get; init; }

    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; set; }

    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public int Rate { get; init; }
}