using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.SectionItems.Commands;

public record CreateSectionItemResponse
{
    [JsonConverter(typeof(SectionItemEncryptor))]
    public int Id { get; init; }
}
