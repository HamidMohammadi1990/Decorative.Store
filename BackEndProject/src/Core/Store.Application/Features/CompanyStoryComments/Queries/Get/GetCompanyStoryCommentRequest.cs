using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public record GetCompanyStoryCommentRequest : IRequest<OperationResult<GetCompanyStoryCommentResponse?>>
{
    [JsonConverter(typeof(CompanyStoryCommentEncryptor))]
    public int Id { get; init; }
}
