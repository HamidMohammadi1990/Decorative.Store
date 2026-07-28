using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStoryLikes.Commands;

public record CreateCompanyStoryLikeRequest : IRequest<OperationResult<CreateCompanyStoryLikeResponse>>
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }
}
