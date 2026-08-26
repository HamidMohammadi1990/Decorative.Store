using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.SectionTypes.Commands;

public record UpdateSectionTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int Id { get; init; }
    public int LanguageId { get; init; }
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
}
