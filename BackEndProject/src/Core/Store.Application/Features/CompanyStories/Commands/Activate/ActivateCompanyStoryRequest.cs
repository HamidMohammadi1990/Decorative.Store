using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStories.Commands;

public record ActivateCompanyStoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int Id { get; init; }
}
