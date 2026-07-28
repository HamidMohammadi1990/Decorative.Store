using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public record ApproveCompanyStoryCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyStoryCommentEncryptor))]
    public int Id { get; init; }
}
