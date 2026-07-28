using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyComments.Queries;

public record GetCompanyCommentRequest : IRequest<OperationResult<GetCompanyCommentResponse>>
{
    [JsonConverter(typeof(CompanyCommentEncryptor))]
    public int Id { get; init; }
}