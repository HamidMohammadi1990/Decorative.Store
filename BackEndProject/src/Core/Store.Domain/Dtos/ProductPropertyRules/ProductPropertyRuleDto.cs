namespace Store.Domain.Dtos.ProductPropertyRules;

public record ProductPropertyRuleDto
{
    public bool IsMandatory { get; set; }
    public string? Description { get; set; }
}

public record NumericProductPropertyRuleDto : ProductPropertyRuleDto
{
    public decimal MinQuantity { get; set; }
    public decimal MaxQuantity { get; set; }
}

public record TextProductPropertyRuleDto : ProductPropertyRuleDto
{
	public int MinLength { get; set; }
	public int MaxLength { get; set; }
}

public record DimensionsProductPropertyRuleDto : ProductPropertyRuleDto
{
    public decimal MinWidth { get; set; }
    public decimal MaxWidth { get; set; }
    public decimal MinHeight { get; set; }
    public decimal MaxHeight { get; set; }
}