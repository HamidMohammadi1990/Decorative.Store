using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.Pages.Commands;

public record DeletePageRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PageEncryptor))]
    public int Id { get; init; }
}
