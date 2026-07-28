using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.SectionTypes.Queries;

public record SearchSectionTypeResponse
{
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int Id { get; init; }
    public string Name { get; init; } = default!;
}
