using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SubCategory : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public int CategoryId { get; private set; }
    public bool IsActive { get; private set; } = true;


    public Category Category { get; private set; } = null!;
    public ICollection<Product> Products { get; private set; } = [];
    public ICollection<Discount> Discounts { get; private set; } = [];


    public static SubCategory Create(string title, string slug, string code, int categoryid)
        => new()
        {
            Title = title,
            Slug = slug,
            Code = code,
            CategoryId = categoryid
        };

    public void AddProducts(List<Product> products)
    {
        foreach (var product in products)
            Products.Add(product);
    }

    public void Update(string title, string slug, string code, int categoryId, bool isActive)
    {
        Code = code;
        Slug = slug;
        Title = title;
        IsActive = isActive;
        CategoryId = categoryId;
    }
}