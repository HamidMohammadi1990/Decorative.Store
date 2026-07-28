using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.Pages.Queries;

public record GetPageRequest : IRequest<OperationResult<GetPageResponse?>>
{
    [JsonConverter(typeof(PageEncryptor))]
    public int Id { get; init; }
}
