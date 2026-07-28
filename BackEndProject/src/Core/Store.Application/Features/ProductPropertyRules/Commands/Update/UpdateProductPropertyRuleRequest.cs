using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductPropertyRules.Commands;

public record UpdateProductPropertyRuleRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductPropertyRuleEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int ProductPropertyId { get; init; }

    public bool IsMandatory { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public int? MinLength { get; init; }
    public int? MaxLength { get; init; }
    public decimal? MinQuantity { get; init; }
    public decimal? MaxQuantity { get; init; }
    public decimal? MinWidth { get; init; }
    public decimal? MaxWidth { get; init; }
    public decimal? MinHeight { get; init; }
    public decimal? MaxHeight { get; init; }
}
