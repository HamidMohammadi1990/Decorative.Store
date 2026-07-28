using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStories.Queries;

public record GetCompanyStoryRequest : IRequest<OperationResult<GetCompanyStoryResponse?>>
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int Id { get; init; }
}
