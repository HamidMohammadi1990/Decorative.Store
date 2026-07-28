using System.Text.Json.Serialization;
using Edition.Application.Features.CompanyStories;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStories.Commands;

public record CreateCompanyStoryRequest : IRequest<OperationResult<CreateCompanyStoryResponse>>
{
    [JsonConverter(typeof(CompanyEncryptor))]
    public int CompanyId { get; init; }

    public string? Caption { get; init; }
    public DateTime? ExpiresAtUtc { get; init; }
    public List<CompanyStoryItemInput> Items { get; init; } = [];
}
