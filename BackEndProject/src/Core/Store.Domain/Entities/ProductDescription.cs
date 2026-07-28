using Store.Domain.Common;

namespace Store.Domain.Entities;

public class ProductDescription : BaseEntity
{
    public string Description { get; private set; } = default!;
    public int ProductId { get; private set; }


    public Product Product { get; private set; } = default!;


    public static ProductDescription Create(string description, int productId)
        => new()
        {
            ProductId = productId,
            Description = description
        };

    public void Update(string description, int productId)
    {
        Description = description;
        ProductId = productId;
    }
}