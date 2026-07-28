using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.PageSections.Commands;

public record CreatePageSectionResponse
{
    [JsonConverter(typeof(PageSectionEncryptor))]
    public int Id { get; init; }
}
