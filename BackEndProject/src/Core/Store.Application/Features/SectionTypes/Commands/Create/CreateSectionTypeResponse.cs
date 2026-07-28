using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.SectionTypes.Commands;

public record CreateSectionTypeResponse
{
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int Id { get; init; }
}
