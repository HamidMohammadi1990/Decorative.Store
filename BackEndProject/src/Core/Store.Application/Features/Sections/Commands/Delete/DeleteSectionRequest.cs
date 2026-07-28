using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.Sections.Commands;

public record DeleteSectionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(SectionEncryptor))]
    public int Id { get; init; }
}
