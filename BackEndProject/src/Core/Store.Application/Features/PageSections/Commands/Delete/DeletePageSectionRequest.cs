using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.PageSections.Commands;

public record DeletePageSectionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PageSectionEncryptor))]
    public int Id { get; init; }
}
