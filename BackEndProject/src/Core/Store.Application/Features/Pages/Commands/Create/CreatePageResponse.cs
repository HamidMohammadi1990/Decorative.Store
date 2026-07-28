using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Pages.Commands;

public record CreatePageResponse
{
    [JsonConverter(typeof(PageEncryptor))]
    public int Id { get; init; }
}
