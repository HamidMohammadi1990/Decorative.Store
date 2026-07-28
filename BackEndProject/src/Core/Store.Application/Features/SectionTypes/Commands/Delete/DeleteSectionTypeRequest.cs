using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.SectionTypes.Commands;

public record DeleteSectionTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int Id { get; init; }
}
