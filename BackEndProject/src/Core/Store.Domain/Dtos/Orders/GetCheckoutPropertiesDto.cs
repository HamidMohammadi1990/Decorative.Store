using Store.Domain.Dtos.ProductPropertyRules;
using Store.Domain.Enums;

namespace Store.Domain.Dtos.Orders;

public record CheckoutPropertiesDto
{
    public string CategoryTitle { get; set; } = null!;
    public List<CheckoutPropertyDto> Properties { get; set; } = [];
}

public record CheckoutPropertItemDependencyDto
{
    public int ParentId { get; set; }
    public int DependentId { get; set; }
}

public record CheckoutPropertyItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int? ItemPriceId { get; set; }
    public decimal? ItemPrice { get; set; }
    public decimal? ItemCooperationPrice { get; set; }
}

public record CheckoutPropertyDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public PropertyType PropertyType { get; set; }
    public List<CheckoutPropertyItemDto> Items { get; set; } = [];
    public List<CheckoutPropertItemDependencyDto> Dependencies { get; set; } = [];
    public List<CheckoutPropertyDto>? Parents { get; set; } = [];
    public ProductPropertyRuleDto? Rule { get; set; }
    public int? PropertyPriceId { get; set; }
    public decimal? PropertyPrice { get; set; }
    public decimal? PropertyCooperationPrice { get; set; }
}