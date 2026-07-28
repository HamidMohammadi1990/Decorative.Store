using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public record DeleteProductPropertyRuleRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductPropertyRuleEncryptor))]
    public int Id { get; init; }
}
