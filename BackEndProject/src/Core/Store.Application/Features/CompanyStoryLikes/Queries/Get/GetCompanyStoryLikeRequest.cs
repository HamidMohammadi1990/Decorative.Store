using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public record GetCompanyStoryLikeRequest : IRequest<OperationResult<GetCompanyStoryLikeResponse?>>
{
    [JsonConverter(typeof(CompanyStoryLikeEncryptor))]
    public int Id { get; init; }
}
