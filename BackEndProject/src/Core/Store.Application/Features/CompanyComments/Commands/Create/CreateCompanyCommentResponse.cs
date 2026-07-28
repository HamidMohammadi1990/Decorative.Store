using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.CompanyComments.Commands;

public record CreateCompanyCommentResponse
{
    [JsonConverter(typeof(CompanyCommentEncryptor))]
    public int Id { get; init; }
}