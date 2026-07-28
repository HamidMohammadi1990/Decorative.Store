using System.Text.Json.Serialization;
using Edition.Application.Features.CompanyStories;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.CompanyStories.Commands;

public record UpdateCompanyStoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int Id { get; init; }

    public string? Caption { get; init; }
    public DateTime? ExpiresAtUtc { get; init; }
    public List<CompanyStoryItemInput> Items { get; init; } = [];
}
