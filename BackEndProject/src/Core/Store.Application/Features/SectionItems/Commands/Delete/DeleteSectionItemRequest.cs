using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.SectionItems.Commands;

public record DeleteSectionItemRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(SectionItemEncryptor))]
    public int Id { get; init; }
}
