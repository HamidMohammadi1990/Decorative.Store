using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.Sections.Commands;

public record CreateSectionResponse
{
    [JsonConverter(typeof(SectionEncryptor))]
    public int Id { get; init; }
}
