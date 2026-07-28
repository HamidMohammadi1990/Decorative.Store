using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public record CreateProductPropertyRuleResponse
{
    [JsonConverter(typeof(ProductPropertyRuleEncryptor))]
    public int Id { get; init; }
}
